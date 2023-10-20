using System;
using System.Windows.Media;

namespace Infections.Models.Health;

public interface IHealthState
{
    Severity InfectionSeverity { get; }
    int Health { get; protected set; }
    double Resistance { get; }
    double InfectionRadius { get; }
    Brush Color { get; }

    IHealthState Progress();

    void Infect(Person other)
    {
        if (other.HealthState is Infected or HealthyInfectious or Incubating) return;
        if (other.HealthState is Recovered recovered && recovered.RecoveredSince < TimeSpan.FromSeconds(60))
            return;

        if (RandomNumberGen.GetInteger(InfectionSeverity.MinThrow, InfectionSeverity.MaxThrow) >= other.Resistance)
        {
            bool saveThrow = RandomNumberGen.GetInteger(1, 100) >= 95;
            if (saveThrow) other.HealthState = new HealthyInfectious(other.Health);
            else other.HealthState = new Incubating(other.Health);
        }
    }
}

public struct Severity
{
    public int MinThrow;
    public int MaxThrow;
    public int SafeThrow;
}