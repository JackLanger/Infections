namespace Infections.Controller;

public interface IPersonObserver
{
    void Register<T>(T person);
}