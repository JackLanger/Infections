using System.Windows.Media;

namespace Infections.Models.Health;

public class Deceased : AbstractHealthState
{
    public Deceased() : base(int.MaxValue,
        0,
        Brushes.Transparent)
    {
    }


    override public IHealthState Progress() => this;
}