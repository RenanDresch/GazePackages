using System.Linq;
using Cysharp.Threading.Tasks;
using Gaze.MVVM.Model;
using Gaze.MVVM.View.Read;
using Gaze.Utilities;
using Gaze.ViewStacking.Services.Read;
using Gaze.ViewStacking.ViewModel.Read;
using UnityEngine;

namespace Gaze.ViewStacking.Services
{
    /// <summary>
    /// Manages a single View Stack
    /// </summary>
    public class ViewStackingService : IViewStackingService
    {
        public ReactiveStack<IStackableViewModel> ViewStack { get; }
        public ReactiveProperty<bool> IsActiveStack { get; } = new ReactiveProperty<bool>();

        readonly Transform stackRoot;
        
        public ViewStackingService(IDestroyable destroyable, Transform stackRoot)
        {
            this.stackRoot = stackRoot;
            ViewStack = new ReactiveStack<IStackableViewModel>();
            SetupStackListeners(destroyable);
        }

        void SetupStackListeners(IDestroyable destroyable)
        {
            IsActiveStack.SafeBindOnChangeAction(destroyable, OnStackStateChange, false);
            ViewStack.SafeBindOnPushAction(destroyable, OnPushView);
            ViewStack.SafeBindOnPopAction(destroyable, OnPopView);
            ViewStack.SafeBindOnClearAction(destroyable, OnStackClear);
        }
        
        async void OnStackClear()
        {
            var stackViews = stackRoot.GetComponentsInChildren<IView>();
            var dismissTasks = Enumerable.Select(stackViews, view => view.Dismiss()).ToList();
            await dismissTasks;
            Object.Destroy(stackRoot.gameObject);
        }

        async void OnStackStateChange(bool isActive)
        {
            if (ViewStack.Count > 0)
            {
                if (isActive)
                {
                    await ViewStack.Peek().Focus();
                }
                else 
                {
                    await ViewStack.Peek().UnFocus();
                }
            }
        }
        
        static async void OnPushView(IStackableViewModel newView, IStackableViewModel formerTopView)
        {
            if (formerTopView != null)
            {
                await formerTopView.UnFocus();
                newView.View.Parent = formerTopView.AttachParent;
            }
            await newView.Present();
        }
        
        static async void OnPopView(IStackableViewModel poppedView, IStackableViewModel newTopView)
        {
            await poppedView.Dismiss();
            Object.Destroy(poppedView.View.GameObject);
            if (newTopView != null)
            {
                await newTopView.Focus();
            }
        }
    }
}
