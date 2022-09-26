namespace Gaze.MVVM
{
    public abstract class ViewModel : IViewModel
    {
        public IView View { get; private set; }

        /// <summary>
        /// Setup the ViewModel instance
        /// </summary>
        /// <param name="view">The View this View Model belongs to</param>
        /// We cannot execute the ViewModel setup within the constructor because Unity blocks too many things during the class serialization
        public virtual void OnStart(IView view)
        {
            View = view;
        }
        
        /// <summary>
        /// Executed when the MonoBehaviour that instantiated this ViewModel is destroyed.
        /// </summary>
        public virtual void OnDestroy() {}

#if UNITY_EDITOR
        public virtual void Reset() {}
        public virtual void OnValidate() {}
#endif
    }
}
