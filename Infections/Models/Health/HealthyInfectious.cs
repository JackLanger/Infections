using System.Windows.Media;

namespace Infections.Models.Health;

public class HealthyInfectious : AbstractHealthState
{
    private int timeTillNonInfectious = int.MaxValue;

    public HealthyInfectious() :
        base(70, 50, Brushes.PaleGoldenrod)
    {
    }

    override public IHealthState Progress() => timeTillNonInfectious-- <= 0 ? new HealthyNonInfectious() : this;
}