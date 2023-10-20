using System;
using System.Windows.Media;

namespace Infections.Models.Health;

public abstract class AbstractHealthState : IHealthState
{
    protected AbstractHealthState(
        int resistance,
        int infectionRadius,
        SolidColorBrush color)
    {
        Resistance = resistance;
        InfectionRadius = infectionRadius;
        Color = color;
    }

    public double Resistance { get; }
    public double InfectionRadius { get; }
    public Brush Color { get; }

    public abstract IHealthState Progress();

    public void Infect(Person other)
    {
        if (other.HealthState is Infected or HealthyInfectious or Incubating) return;
        if (other.HealthState is Recovered recovered && canBeInfected(recovered))
            return;

        int infectionRoll = RandomNumberGen.GetInteger(0, 100);
        if (other.Resistance < infectionRoll)
        {
            bool saveThrow = RandomNumberGen.GetInteger(1, 100) >= 95;
            if (saveThrow) other.HealthState = new HealthyInfectious();
            else other.HealthState = new Incubating();
        }
    }

    private static bool canBeInfected(Recovered recovered) =>
        DateTime.Now-recovered.RecoveredSince < TimeSpan.FromSeconds(180);
}