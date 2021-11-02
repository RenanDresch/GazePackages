namespace Gaze.MVVM.Services
{
    public static class ReactivePropertyExtensions
    {
        public static void SetValue<T>(this ReactiveProperty<T> reactiveProperty, T newValue)
        {
            reactiveProperty.Writer.Value = newValue;
        }
    }
}