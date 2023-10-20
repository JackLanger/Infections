using System.Collections.Generic;
using System.Threading.Tasks;
using Infections.Models;

namespace Infections.Observer;

public class CommuteObserver : IRegisterToObserver
{
    private readonly IList<CommuteTarget> _commuteTargets;

    public CommuteObserver() : this(new List<CommuteTarget>())
    {
    }

    public CommuteObserver(IList<CommuteTarget> targets)
    {
        _commuteTargets = targets;
    }

    public void Register<T>(T obj)
    {
        if (obj is Person person)
        {
            person.PositionChangedEvent += OnPositionChanged;
        }
        else if (obj is CommuteTarget commute)
        {
            _commuteTargets.Add(commute);
        }
    }

    private void OnPositionChanged(Person sender)
    {
        Task.Run(() => { Parallel.ForEach(_commuteTargets, target => { AttractIfInRange(sender, target); }); });
    }

    private static void AttractIfInRange(Person sender, CommuteTarget target)
    {
        if (target.IsWithinRange(sender.Position))
        {
            target.Attract(sender);
        }
    }
}