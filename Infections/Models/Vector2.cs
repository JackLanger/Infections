using System;
using System.Windows;

namespace Infections.Models;

public class Vector2
{
    public double X { get; set; }
    public double Y { get; set; }

    public double Length
    {
        get => Math.Sqrt(X * X+Y * Y);
    }

    public Vector2(double x, double y)
    {
        X = x;
        Y = y;
    }

    public static Vector2 operator +(Vector2 v1, Vector2 v2)
    {
        return new Vector2(v1.X+v2.X, v1.Y+v2.Y);
    }

    public static Vector2 operator -(Vector2 v1, Vector2 v2)
    {
        return new Vector2(v1.X-v2.X, v1.Y-v2.Y);
    }

    public static Vector2 operator *(Vector2 v1, double scalar)
    {
        return new Vector2(v1.X * scalar, v1.Y * scalar);
    }

    public static Vector2 operator *(double scalar, Vector2 v1)
    {
        return new Vector2(v1.X * scalar, v1.Y * scalar);
    }

    public static Vector2 operator /(Vector2 v1, double scalar)
    {
        return new Vector2(v1.X / scalar, v1.Y / scalar);
    }

    public Point ToPoint()
    {
        return new Point(X, Y);
    }

    public Vector2 Normalized()
    {
        return this / Length;
    }
}