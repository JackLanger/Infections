namespace Infections.Models.events;

public record AttractionZoneEnterEventArgs(Vector2 Position, double AttractionValue,double Radius)
{
}