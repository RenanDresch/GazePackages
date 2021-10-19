using System;
using Cysharp.Threading.Tasks;
using Gaze.MVVM.ReadOnly;
using UnityEngine;
using UnityEngine.Events;

namespace Gaze.MVVM
{
    public abstract class ViewModel : IViewModel
    {
        public UnityEvent OnDestroyEvent { get; } = new UnityEvent();
        public IView View { get; private set; }

        bool presented;
        bool dismissed;

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
            try
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
            catch (OperationCanceledException)
            {
                Debug.LogWarning("View destroyed before completing Present.");
            }
            catch (Exception e)
            {
                Debug.LogError(e, View.GameObject);
                throw;
            }
        }

        public async UniTask Focus()
        {
            try
            {
                await View.Focus();
            }
            catch (OperationCanceledException)
            {
                Debug.LogWarning("View destroyed before completing Focus.");
            }
            catch (Exception e)
            {
                Debug.LogError(e, View.GameObject);
                throw;
            }
        }
        
        public async UniTask UnFocus()
        {
            try
            {
                await View.UnFocus();
            }
            catch (OperationCanceledException)
            {
                Debug.LogWarning("View destroyed before completing UnFocus.");
            }
            catch (Exception e)
            {
                Debug.LogError(e, View.GameObject);
                throw;
            }
        }
        
        public async UniTask Dismiss()
        {
            try
            {
                if (!dismissed)
                {
                    dismissed = true;
                    await View.Dismiss();
                }
                else
                {
                    Debug.LogWarning("Attempting to dismiss an already dismissed view", View.GameObject);
                }
            }
            catch (OperationCanceledException)
            {
                Debug.LogWarning("View destroyed before completing dismiss.");
            }
            catch (Exception e)
            {
                Debug.LogError(e, View.GameObject);
                throw;
            }
        }
        
#if UNITY_EDITOR
        public virtual void Reset()
        {
        }
#endif
    }
}
