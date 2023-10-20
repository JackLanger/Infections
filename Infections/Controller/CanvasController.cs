using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Infections.Models;
using Infections.Models.Health;

namespace Infections.Controller;

public class CanvasController
{
    private const int CommuteTargets = 4;
    private readonly Canvas _canvas;
    private readonly IList<CommuteTarget> _commuteTargets = new List<CommuteTarget>();
    private readonly PersonObserver _personObserver;
    private readonly IList<Person> _persons = new List<Person>();
    private ICommand _startCommand;

    public CanvasController(Canvas canvas)
    {
        _canvas = canvas;
        _personObserver = new PersonObserver();
        InitCommuteTargets();
        InitPersons();
    }

    public ICommand StartCommand
    {
        get => _startCommand ??= new DelegateCommand(StartLoop);
    }

    private void InitCommuteTargets()
    {
        int target = CommuteTargets / 2;
        for (int i = 0; i < target; i++)
        for (int j = 0; j < target; j++)
        {
            double attraction = RandomNumberGen.GetDouble(30, 300);

            CommuteTarget commuteTarget =
                new CommuteTarget(RandomNumberGen.GetVectorWithin(i * 300+100, (i+1) * 300+100, j * 100+100,
                    (j+1) * 200+100));
            _commuteTargets.Add(commuteTarget);
            Draw(commuteTarget.Outline);
        }
    }


    private void InitPersons()
    {
        int personCount = 100;
        for (int i = 0; i < personCount; i++)
        {
            Person person = new Person(RandomNumberGen.GetRandomVector2(), RandomNumberGen.GetDouble());
            foreach (CommuteTarget commuteTarget in _commuteTargets)
            {
                person.RegisterCommuteTarget(commuteTarget);
            }
            _persons.Add(person);
            Draw(person.Geometry);
            _personObserver.Register(person);
            person.HealthStateChanged += Person_HealthStateChanged;
        }

        for (int i = 0; i < 1; i++)
        {
            Person target = _persons[RandomNumberGen.GetInteger(0, _persons.Count-1)];
            target.HealthState = new HealthyInfectious(target.Health);
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
        var remove = new List<Person>();
        _personObserver.Start();
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
            Draw(person.Geometry);
        }
    }

    private void ProcessAllPersons(List<Person> remove)
    {
        foreach (Person person in _persons)
        {
            if (person.HealthState is Deceased)
            {
                remove.Add(person);
            }
            else
            {
                Task.Run(person.Update);
                Application.Current.Dispatcher.BeginInvoke(() => UpdatePerson(person));
            }
        }
    }

    private void UpdatePerson(Person person)
    {
        Vector2 personPosition = person.Position;
        person.Geometry.Margin = new Thickness(personPosition.X, personPosition.Y, 0, 0);
        Draw(person.Geometry);
    }


    private void RemoveDeceased(List<Person> remove)
    {
        foreach (Person person in remove)
        {
            _persons.Remove(person);
            _canvas.Children.Remove(person.Geometry);
        }
    }
}