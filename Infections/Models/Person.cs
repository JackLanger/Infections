using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Infections.Models.Health;

namespace Infections.Models;

public class Person : IHealthState
{
    public Person() : this(new Vector2(0, 0), 1)
    {
    }

    public Person(Vector2 position, double speed)
    {
        HealthState = new HealthyNonInfectious(Health);
        Position = position;
        Speed = speed;
        Velocity = RandomNumberGen.GetRandomVector2();
        Health = RandomNumberGen.GetInteger(50, 100);
        Geometry = CreateGeometry();
    }

    public event AttractionZoneEnterEventHandler AttractionZoneEntered;
    public event HealthStateChangedEventHandler HealthStateChanged;

    #region Properties

    private IHealthState _healthState;

    public IHealthState HealthState
    {
        get => _healthState;
        set
        {
            _healthState = value;
            HealthStateChanged?.Invoke(this);
        }
    }

    public Vector2 Position { get; set; }
    public double Speed { get; set; }
    public Vector2 Velocity { get; set; }

    private readonly IList<CommuteTarget> _registeredTargets = new List<CommuteTarget>();

    public double Resistance
    {
        get => HealthState.Resistance;
    }

    public double InfectionRadius
    {
        get => HealthState.InfectionRadius;
    }

    public Brush Color
    {
        get => HealthState.Color;
    }

    public Ellipse Geometry { get; }

    public Severity InfectionSeverity
    {
        get => HealthState.InfectionSeverity;
    }

    public int Health { get; set; }

    public bool IsDead
    {
        get => HealthState is Deceased;
    }

    private Ellipse CreateGeometry()
    {
        Ellipse ellipse = new Ellipse();
        ellipse.Height = ellipse.Width = 10;
        ellipse.Stroke = ellipse.Fill = Color;
        ellipse.StrokeThickness = 1;
        ellipse.Margin = new Thickness(Position.X, Position.Y, 0, 0);
        ellipse.HorizontalAlignment = HorizontalAlignment.Center;
        ellipse.VerticalAlignment = VerticalAlignment.Center;
        return ellipse;
    }

    #endregion


    #region Public Methods

    public void Update()
    {
        CheckForAttractionZones();
        Progress();
        Vector2 velo = Velocity.Normalized();
        Position.X += velo.X * Speed;
        Position.Y += velo.Y * Speed;

        if (Position.X < 0 || Position.X > 800 || Position.Y < 0 || Position.Y > 600)
            Velocity *= -1;
    }

    private void CheckForAttractionZones()
    {
        foreach (CommuteTarget target in _registeredTargets)
        {
            if ((target.Position-Position).Length < target.AttractionRadius)
            {
                AttractionZoneEntered?.Invoke(this);
            }
        }
    }

    public void RegisterCommuteTarget(CommuteTarget target)
    {
        _registeredTargets.Add(target);
        AttractionZoneEntered += target.Attract;
    }

    public IHealthState Progress()
    {
        IHealthState state = HealthState.Progress();
        Type type = HealthState.GetType();
        bool isSameType = state.GetType() == type;
        if (!isSameType) HealthState = state;
        return state;
    }

    public void Infect(Person other) => HealthState.Infect(other);

    #endregion
}

public delegate void HealthStateChangedEventHandler(Person sender);

public delegate void AttractionZoneEnterEventHandler(Person sender);