using System;
using System.Windows.Media;

namespace Infections.Models.Health;

public class Incubating : IHealthState
{
    private readonly DateTime _timeInfected = DateTime.Now;

    public Incubating(int otherHealth)
    {
        Health = otherHealth;
    }

    public IHealthState Progress() =>
        DateTime.Now-_timeInfected < TimeSpan.FromSeconds(4) ? this : new Infected(Health);

    public Severity InfectionSeverity { get; } = new Severity { MinThrow = 10, MaxThrow = 80, SafeThrow = 60 };

    public int Health { get; set; }
    public double Resistance { get; } = 0;
    public double InfectionRadius { get; } = 30;
    public Brush Color { get; } = Brushes.Blue;
}