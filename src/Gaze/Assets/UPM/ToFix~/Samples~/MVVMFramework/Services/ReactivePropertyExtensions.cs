using Gaze.MVVM;

namespace MVVM.Services
{
    internal static class ReactivePropertyExtensions
    {
        internal static void SetValue<T>(this ReactiveProperty<T> reactiveProperty, T newValue)
        {
            reactiveProperty.Writer.Value = newValue;
        }
    }
}