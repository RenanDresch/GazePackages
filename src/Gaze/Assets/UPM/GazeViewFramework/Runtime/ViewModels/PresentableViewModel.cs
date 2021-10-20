using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Gaze.MVVM.ViewStacking
{
    [Serializable]
    public abstract class PresentableViewModel : ViewModel, IPresentableViewModel
    {
        protected IPresentableView presentableView;
        
        bool presented;
        bool dismissed;

        public virtual UniTask SetupPresentableView(IPresentableView presentableView)
        {
            this.presentableView = presentableView;
            return UniTask.CompletedTask;
        }
        
        public async UniTask Present()
        {
            try
            {
                if (!presented)
                {
                    presented = true;
                    await presentableView.Present();
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
                await presentableView.Focus();
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
                await presentableView.UnFocus();
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
                    await presentableView.Dismiss();
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
    }
}