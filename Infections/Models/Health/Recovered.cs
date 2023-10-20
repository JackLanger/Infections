using System;
using System.Windows.Media;

namespace Infections.Models.Health;

public class Recovered : IHealthState
{
    private DateTime recoveredOn;

    public Recovered(int health)
    {
        Health = health;
        recoveredOn = DateTime.Now;
    }

    public TimeSpan RecoveredSince { get; }

    public IHealthState Progress() => this;

    public Severity InfectionSeverity { get; } = new Severity { MinThrow = 0, MaxThrow = 0, SafeThrow = 100 };

    public int Health { get; set; }
    public double Resistance { get; } = 97;
    public double InfectionRadius { get; } = 0;
    public Brush Color { get; } = Brushes.Aquamarine;
}