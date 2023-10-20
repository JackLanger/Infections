using System.Windows.Media;

namespace Infections.Models.Health;

public class HealthyInfectious : AbstractHealthState
{
    private int _timeTillNonInfectious = int.MaxValue;

    public HealthyInfectious() :
        base(70, 50, Brushes.PaleGoldenrod)
    {
    }

    override public IHealthState Progress() => _timeTillNonInfectious-- <= 0 ? new HealthyNonInfectious() : this;
}