using System.Collections.Generic;
using System.Threading.Tasks;
using Infections.Models;
using Infections.Models.Health;

namespace Infections.Controller;

public class RegisterToObserver : IRegisterToObserver
{
    private readonly IList<Person> _sickList = new List<Person>();

    public void Register<T>(T target)
    {
        if (target is Person person)
        {
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
                    sender.HealthStateChanged -= OnPersonHealthStateChanged;
                    sender.PositionChangedEvent -= OnPersonPositionChanged;
                    return;
                case Recovered:
                    _sickList.Remove(sender);
                    break;
                case Infected:
                case HealthyInfectious:
                    _sickList.Add(sender);
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