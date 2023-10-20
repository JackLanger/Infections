using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Infections.Models;

public class CommuteTarget
{
    public CommuteTarget(Vector2 position)
    {
        Position = position;
        AttractionRadius = RandomNumberGen.GetDouble(30, 300);
        AttractionValue = RandomNumberGen.GetDouble(1, AttractionRadius);
        Outline = CreateGeometry();
    }

    public Ellipse Outline { get; }

    public double AttractionValue { get; set; }

    public double AttractionRadius { get; set; }

    public Vector2 Position { get; set; }

    public Ellipse CreateGeometry()
    {
        Ellipse ellipse = new Ellipse();
        ellipse.Height = ellipse.Width = AttractionRadius;
        ellipse.Fill = Brushes.Transparent;
        ellipse.Stroke = Brushes.OrangeRed;
        ellipse.StrokeDashArray = new DoubleCollection(new double[] { 5, 5 });
        ellipse.StrokeThickness = 1;
        ellipse.Margin = new Thickness(Position.X, Position.Y, 0, 0);
        ellipse.HorizontalAlignment = HorizontalAlignment.Center;
        ellipse.VerticalAlignment = VerticalAlignment.Center;
        return ellipse;
    }

    public void Attract(Person person)
    {
        Vector2 direction = Position-person.Position;
        double length = direction.Length;
        double magnitude = AttractionValue / length;
        person.Velocity += magnitude * direction;
    }

    public bool IsWithinRange(Vector2 location) => (location-Position).Length <= AttractionRadius;
}