using System.Collections.Generic;
using System.Threading.Tasks;
using Infections.Models;

namespace Infections.Controller;

public class CommuteObserver : IPersonObserver
{
    private readonly IList<CommuteTarget> _commuteTargets;

    private readonly IList<Person> _observedPersons;
    private bool _isObserving;

    public CommuteObserver() : this(new List<CommuteTarget>())
    {
    }

    public CommuteObserver(IList<CommuteTarget> targets)
    {
        _observedPersons = new List<Person>();
        _commuteTargets = targets;
    }

    public void Register<T>(T obj)
    {
        if (obj is Person person)
        {
            _observedPersons.Add(person);
            person.PositionChangedEvent += OnPositionChanged;
        }
        else if (obj is CommuteTarget commute)
        {
            _commuteTargets.Add(commute);
        }
    }

    private void OnPositionChanged(Person sender)
    {
        Task.Run(() => AttractPerson(sender));
    }

    private void AttractPerson(Person sender)
    {
        Parallel.ForEach(_commuteTargets, target =>
        {
            if (target.IsWithinRange(sender.Position))
            {
                target.Attract(sender);
            }
        });
    }

    public void Register(Person person)
    {
        _observedPersons.Add(person);
    }
}