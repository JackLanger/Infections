using System.Windows.Media;

namespace Infections.Models.Health;

public class HealthyInfectious : IHealthState
{
    private int timeTillNonInfectious = int.MaxValue;

    public HealthyInfectious(int health)
    {
        Health = health;
    }

    public IHealthState Progress() => timeTillNonInfectious-- <= 0 ? new HealthyNonInfectious(Health) : this;

    public Severity InfectionSeverity { get; } = new Severity { MinThrow = 0, MaxThrow = 70, SafeThrow = 20 };

    public int Health { get; set; }
    public double Resistance { get; } = 80;
    public double InfectionRadius { get; } = 30;
    public Brush Color { get; } = Brushes.PaleGoldenrod;
}