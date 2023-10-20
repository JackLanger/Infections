using System;
using System.Windows.Media;

namespace Infections.Models.Health;

public class Recovered : AbstractHealthState
{
    public Recovered() : base(
        99, 0, Brushes.Aquamarine)
    {
        RecoveredSince = DateTime.Now;
    }

    public DateTime RecoveredSince { get; }

    override public IHealthState Progress()
    {
        if (DateTime.Now-RecoveredSince > TimeSpan.FromSeconds(30))
            return new HealthyNonInfectious();
        return this;
    }
}