using Cysharp.Threading.Tasks;
using Gaze.MVVM.View.Read;
using Gaze.MVVM.ViewModel.Read;
using UnityEngine;
using UnityEngine.Events;

namespace Gaze.MVVM.ViewModel
{
    public abstract class ViewModel : IViewModel
    {
        public UnityEvent OnDestroyEvent { get; } = new UnityEvent();
        public IView View { get; private set; }

        bool presented;
        
        /// <summary>
        /// Setup the ViewModel instance
        /// </summary>
        /// <param name="view">The View this View Model belongs to</param>
        /// We cannot execute the ViewModel setup within the constructor because Unity blocks too many things during the class serialization
        public virtual async UniTask Setup(IView view)
        {
            View = view;
            await SetupServices();
        }

        protected virtual async UniTask SetupServices()
        {
            await UniTask.CompletedTask;
        }

        public async UniTask Present()
        {
            if (!presented)
            {
                presented = true;
                await View.Present();
            }
            else
            {
                Debug.LogWarning("Attempting to present an already presented view", View.GameObject);
            }
        }

        public async UniTask Focus()
        {
            await View.Focus();
        }
        
        public async UniTask UnFocus()
        {
            await View.UnFocus();
        }
        
        public async UniTask Dismiss()
        {
            if (presented)
            {
                presented = false;
                await View.Dismiss();
            }
            else
            {
                Debug.LogWarning("Attempting to dismiss an already dismissed view", View.GameObject);
            }
        }
        
#if UNITY_EDITOR
        public virtual void Reset()
        {
        }
#endif
    }
}
