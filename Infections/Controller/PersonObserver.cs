using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Infections.Models;
using Infections.Models.Health;

namespace Infections.Controller;

public class PersonObserver
{
    private readonly IList<Person> _observedPersons;
    private readonly IList<Person> _sickList;
    private bool isAlive;

    public PersonObserver()
    {
        _observedPersons = new List<Person>();
        _sickList = new List<Person>();
        ObservePersons();
    }

    public void Cancel() => isAlive = false;

    public void Start() => isAlive = true;

    private void ObservePersons()
    {
        Task.Run(() =>
        {
            while (isAlive)
            {
                var sickListCopy = new Person[_sickList.Count];
                _sickList.CopyTo(sickListCopy, 0);
                foreach (Person sick in sickListCopy)
                {
                    InfectOthers(sick);
                }
                Thread.Sleep(1000);
                sickListCopy = null;
            }
        });
    }


    public void Register(Person person)
    {
        _observedPersons.Add(person);
        person.HealthStateChanged += Person_HealthStateChanged;
    }

    private void Person_HealthStateChanged(Person sender)
    {
        switch (sender.HealthState)
        {
            case Deceased:
                _sickList.Remove(sender);
                _observedPersons.Remove(sender);
                sender.HealthStateChanged -= Person_HealthStateChanged;
                return;
            case Recovered:
                _sickList.Remove(sender);
                _observedPersons.Add(sender);
                sender.HealthStateChanged -= Person_HealthStateChanged;
                break;
            case Infected:
            case HealthyInfectious:
                _sickList.Add(sender);
                _observedPersons.Remove(sender);
                Task.Run(() => InfectOthers(sender));
                break;
        }
    }

    private void InfectOthers(Person sender)
    {
        var copy = new Person[_observedPersons.Count];
        _observedPersons.CopyTo(copy, 0);
        foreach (Person other in copy)
        {
            if (other == sender) continue;

            if (IsInInfectionRange(sender, other))
            {
                sender.Infect(other);
            }
        }
    }

    private bool IsInInfectionRange(Person sender, Person? other)
    {
        if (other is null) return false;

        return (other!.Position-sender.Position).Length <= sender.InfectionRadius;
    }
}