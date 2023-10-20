using System.Collections.Generic;
using System.Threading.Tasks;
using Infections.Models;
using Infections.Models.Health;

namespace Infections.Controller;

public class PersonObserver : IPersonObserver
{
    private readonly IList<Person> _observedPersons = new List<Person>();
    private readonly IList<Person> _sickList = new List<Person>();
    private bool _isAlive;

    public void Register<T>(T target)
    {
        if (target is Person person)
        {
            _observedPersons.Add(person);
            person.HealthStateChanged += OnPersonHealthStateChanged;
            person.PositionChangedEvent += OnPersonPositionChanged;
        }
    }

    private void OnPersonPositionChanged(Person sender)
    {
        Person[] sickListCopy;
        lock (_sickList)
        {
            sickListCopy = new Person[_sickList.Count];
            _sickList.CopyTo(sickListCopy, 0);
        }

        Task.Run(() =>
        {
            foreach (Person sick in sickListCopy)
            {
                Person sickC = sick;
                if (sender != sick && IsInInfectionRange(ref sickC, sender))
                {
                    sick.Infect(sender);
                }
            }
        });
    }


    private void OnPersonHealthStateChanged(Person sender)
    {
        lock (_sickList)
        {
            switch (sender.HealthState)
            {
                case Deceased:
                    _sickList.Remove(sender);
                    _observedPersons.Remove(sender);
                    sender.HealthStateChanged -= OnPersonHealthStateChanged;
                    sender.PositionChangedEvent -= OnPersonPositionChanged;
                    return;
                case Recovered:
                    _sickList.Remove(sender);
                    _observedPersons.Add(sender);
                    break;
                case Infected:
                case HealthyInfectious:
                    _sickList.Add(sender);
                    _observedPersons.Remove(sender);
                    break;
            }
        }
    }


    private bool IsInInfectionRange(ref Person sick, Person? other)
    {
        if (other is null) return false;

        return (other!.Position-sick.Position).Length <= sick.InfectionRadius;
    }
}