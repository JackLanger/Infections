namespace Infections.Controller;

public interface IRegisterToObserver
{
    void Register<T>(T person);
}