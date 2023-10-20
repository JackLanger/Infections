using System.Windows.Media;

namespace Infections.Models.Health;

public class Deceased : IHealthState
{
    public IHealthState Progress() => this;

    public Severity InfectionSeverity { get; } = new Severity { MinThrow = 0, MaxThrow = 0, SafeThrow = 100 };

    public int Health { get; set; } = 0;
    public double Resistance { get; } = int.MaxValue;
    public double InfectionRadius { get; } = 0;
    public Brush Color { get; } = Brushes.Transparent;
}