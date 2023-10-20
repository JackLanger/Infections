namespace Infections.Observer;

public interface IRegisterToObserver
{
    void Register<T>(T person);
}