using System;
using System.Windows.Media;

namespace Infections.Models.Health;

public class Infected : AbstractHealthState
{
    private readonly DateTime _timeInfected;
    private bool? _isDying;

    public Infected() : base(
        0, 80, Brushes.Red)
    {
        _timeInfected = DateTime.Now;
    }

    private bool IsDying
    {
        get => _isDying ??= RandomNumberGen.GetDouble() >= .9;
    }

    override public IHealthState Progress()
    {
        if (DateTime.Now-_timeInfected > TimeSpan.FromSeconds(10))
        {
            return IsDying ? new Deceased() : new Recovered();
        }

        return this;
    }
}