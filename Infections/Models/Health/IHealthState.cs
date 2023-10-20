using System.Windows.Media;

namespace Infections.Models.Health;

public interface IHealthState
{
    double Resistance { get; }
    double InfectionRadius { get; }
    Brush Color { get; }

    IHealthState Progress();

    void Infect(Person other);
}