using System;
using System.Windows.Media;

namespace Infections.Models.Health;

public class Infected : IHealthState
{
    private readonly DateTime timeInfected;

    public Infected(int health)
    {
        Health = health;
        timeInfected = DateTime.Now;
    }

    public IHealthState Progress()
    {
        Health -= 5;

        if (Health <= 0)
        {
            return new Deceased();
        }

        return DateTime.Now-timeInfected < TimeSpan.FromSeconds(10) ? this : new Recovered(Health);
    }

    public Severity InfectionSeverity { get; } = new Severity { MinThrow = 50, MaxThrow = 100, SafeThrow = 80 };
    public int Health { get; set; }

    public double Resistance { get; } = 0;
    public double InfectionRadius { get; } = 80;
    public Brush Color { get; } = Brushes.Red;
}