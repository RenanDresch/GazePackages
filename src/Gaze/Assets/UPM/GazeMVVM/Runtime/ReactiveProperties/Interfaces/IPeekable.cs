namespace Gaze.MVVM
{
    public interface IPeekable<out T>
    {
        IReactiveProperty<T> Peek { get; }
    }
}