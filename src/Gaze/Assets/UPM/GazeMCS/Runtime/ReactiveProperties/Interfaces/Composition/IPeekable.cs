namespace Gaze.MCS
{
    public interface IPeekable<T>
    {
        IReactiveProperty<T> Peek { get; }
    }
}