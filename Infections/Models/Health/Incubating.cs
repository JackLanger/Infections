using System;
using System.Windows.Media;

namespace Infections.Models.Health;

public class Incubating : AbstractHealthState
{
    private readonly DateTime _timeInfected = DateTime.Now;

    public Incubating() : base(
        0, 30, Brushes.Blue)
    {
    }


    override public IHealthState Progress() =>
        DateTime.Now-_timeInfected < TimeSpan.FromSeconds(4)
            ? this
            : new Infected();
}