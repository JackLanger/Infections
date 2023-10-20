using System.Windows.Media;

namespace Infections.Models.Health;

public class HealthyNonInfectious : IHealthState
{
    public HealthyNonInfectious(int health)
    {
        Health = health;
    }

    public IHealthState Progress() => this;

    public Severity InfectionSeverity { get; } = new Severity { MinThrow = 0, MaxThrow = 0, SafeThrow = 100 };
    public int Health { get; set; }
    public double Resistance { get; } = 50;
    public double InfectionRadius { get; } = 0;
    public Brush Color { get; } = Brushes.White;
}