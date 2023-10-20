using System;
using System.Windows.Media;

namespace Infections.Models.Health;

public class HealthyNonInfectious : AbstractHealthState
{
    private readonly DateTime _dateOfInfection;

    public HealthyNonInfectious() : base(
        70, 0, Brushes.White)
    {
        _dateOfInfection = DateTime.Now;
    }

    override public IHealthState Progress() =>
        DateTime.Now-_dateOfInfection < TimeSpan.FromSeconds(15) ? this : new HealthyNonInfectious();
}