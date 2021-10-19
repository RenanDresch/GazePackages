using Cysharp.Threading.Tasks;
using Gaze.MVVM.ReadOnly;

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
        public virtual UniTask Setup(IView view)
        {
            View = view;
            return UniTask.CompletedTask;
        }

#if UNITY_EDITOR
        public virtual void Reset()
        {
        }
#endif
    }
}
