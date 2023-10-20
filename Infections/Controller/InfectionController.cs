using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Infections.Models;
using Infections.Models.Health;
using Infections.Observer;

namespace Infections.Controller;

public class InfectionController
{
    private const int CommuteTargets = 4;
    private const int PersonCount = 100;
    private readonly Canvas _canvas;
    private readonly IRegisterToObserver _commuteObserver;
    private readonly IList<CommuteTarget> _commuteTargets;
    private readonly IList<Person> _persons;
    private readonly IRegisterToObserver _registerToObserver;
    private bool _isRunning;
    private ICommand? _startCommand;

    public InfectionController(Canvas canvas)
    {
        _canvas = canvas;
        _registerToObserver = new RegisterToObserver();
        _commuteObserver = new CommuteObserver();
        _persons = new List<Person>();
        _commuteTargets = new List<CommuteTarget>();
        InitCommuteTargets();
        InitPersons();
    }

    public ICommand StartCommand
    {
        get => _startCommand ??= new DelegateCommand(() =>
        {
            if (!_isRunning)
                StartLoop();
        });
    }

    private void InitCommuteTargets()
    {
        int target = CommuteTargets / 2;
        for (int i = 0; i < target; i++)
        for (int j = 0; j < target; j++)
        {
            CommuteTarget commuteTarget =
                new CommuteTarget(RandomNumberGen.GetVectorWithin(
                    i * 300+100,
                    (i+1) * 300+100,
                    j * 100+100,
                    (j+1) * 200+100)
                );
            _commuteTargets.Add(commuteTarget);
            _commuteObserver.Register(commuteTarget);
            Draw(commuteTarget.Outline);
        }
    }


    private void InitPersons()
    {
        for (int i = 0; i < PersonCount; i++)
        {
            Person person = new Person(RandomNumberGen.GetRandomVector2(), RandomNumberGen.GetDouble(0.5, 2));
            _commuteObserver.Register(person);
            _persons.Add(person);
            if (person.Geometry is not null) Draw(person.Geometry);
            _registerToObserver.Register(person);
            person.HealthStateChanged += Person_HealthStateChanged;
        }

        for (int i = 0; i < 1; i++)
        {
            Person target = _persons[RandomNumberGen.GetInteger(0, _persons.Count-1)];
            target.HealthState = new HealthyInfectious();
        }
    }

    private void Person_HealthStateChanged(Person sender)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            if (sender.Geometry is not null)
                sender.Geometry.Stroke = sender.Geometry.Fill = sender.HealthState.Color;
        });
    }

    private void Draw(UIElement obj)
    {
        Application.Current.Dispatcher.BeginInvoke(() =>
        {
            _canvas.Children.Remove(obj);
            _canvas.Children.Add(obj);
        });
    }

    public void StartLoop()
    {
        _isRunning = true;
        var remove = new List<Person>();

        Task.Run(() =>
        {
            while (_persons.Count > 0)
            {
                remove.Clear();
                ProcessAllPersons(remove);
                RemoveDeceased(remove);
                Application.Current.Dispatcher.BeginInvoke(Update);
                Thread.Sleep(1000 / 60);
            }
        });
    }

    public void Update()
    {
        foreach (CommuteTarget target in _commuteTargets)
        {
            Draw(target.Outline);
        }
        foreach (Person person in _persons)
        {
            if (person.Geometry is not null) Draw(person.Geometry);
        }
    }

    private void ProcessAllPersons(List<Person> remove)
    {
        Parallel.ForEach(_persons, person =>
        {
            if (person.HealthState is Deceased)
            {
                remove.Add(person);
            }
            else
            {
                person.UpdatePosition();
                person.Progress();
                Application.Current.Dispatcher.BeginInvoke(() => UpdatePerson(person));
            }
        });
    }

    private void UpdatePerson(Person person)
    {
        Vector2 personPosition = person.Position;
        if (person.Geometry is not null)
        {
            person.Geometry.Margin = new Thickness(personPosition.X, personPosition.Y, 0, 0);
            Draw(person.Geometry);
        }
    }


    private void RemoveDeceased(List<Person> remove)
    {
        foreach (Person person in remove)
        {
            _persons.Remove(person);
            Application.Current.Dispatcher.BeginInvoke(() => _canvas.Children.Remove(person.Geometry));
        }
    }
}