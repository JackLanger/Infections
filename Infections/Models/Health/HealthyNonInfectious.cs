using System;
using System.Windows.Media;

namespace Infections.Models.Health;

public class HealthyNonInfectious : AbstractHealthState
{
    private readonly DateTime dateOfInfection;

    public HealthyNonInfectious() : base(
        70, 0, Brushes.White)
    {
        dateOfInfection = DateTime.Now;
    }

    override public IHealthState Progress() =>
        DateTime.Now-dateOfInfection < TimeSpan.FromSeconds(15) ? this : new HealthyNonInfectious();
}