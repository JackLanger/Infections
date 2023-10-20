using System;
using System.Windows.Media;

namespace Infections.Models;

public static class RandomNumberGen
{
    private readonly static Random Random = new Random();
    private const double WindowWidth = 800;
    private const double WindowHeight = 600;

    public static int GetInteger(int min = 0, int max = Int32.MaxValue)
    {
        return Random.Next(min, max);
    }

    public static double GetDouble(double min = 0, double max = 1.0)
    {
        return Random.NextDouble() * (max-min)+min;
    }

    public static Vector2 GetVectorWithin(double left,double right, double top, double bottom)
    {
        return new Vector2(
            GetDouble(left, right),
            GetDouble(top, bottom)
            );
    }

    public static Vector2 GetRandomVector2(double offsetFromLeft = 0, double offsetFromTop = 0)
    {
        return new Vector2(
            GetDouble(offsetFromLeft, WindowWidth-offsetFromLeft),
            GetDouble(offsetFromTop, WindowHeight-offsetFromTop)
        );
    }
}