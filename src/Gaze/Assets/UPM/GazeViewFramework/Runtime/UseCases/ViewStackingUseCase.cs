using System.Linq;
using Cysharp.Threading.Tasks;
using Gaze.MVVM.ReadOnly.ViewStacking;
using Gaze.Utilities;
using UnityEngine;

namespace Gaze.MVVM.ViewStacking
{
    /// <summary>
    /// Manages a single View Stack
    /// </summary>
    [CreateAssetMenu(menuName = "Use Cases/"+nameof(ViewStackingUseCase))]
    public class ViewStackingUseCase : ScriptableObject
    {
        [SerializeField]
        ViewStackOrchestrationModel viewStackOrchestrationModel;

        public ReactiveStack<IStackableViewModel> ViewStack { get; private set; }

        public Transform StackRoot { get; private set; }

        IDestroyable stackOwnerObject;
        bool stackDestroyed;
        
        public void SetupInstance(IDestroyable stackOwnerObject, Transform stackRoot)
        {
            this.stackOwnerObject = stackOwnerObject;
            StackRoot = stackRoot;
            ViewStack = new ReactiveStack<IStackableViewModel>();
            SetupStackListeners();
        }

        void SetupStackListeners()
        {
            stackOwnerObject.OnDestroyEvent += OnStackOwnerDestroy;
            viewStackOrchestrationModel.ActiveStackingService.SafeBindOnChangeAction(stackOwnerObject, OnActiveStackChange);
            ViewStack.SafeBindOnPushAction(stackOwnerObject, OnPushView);
            ViewStack.SafeBindOnPopAction(stackOwnerObject, OnPopView);
            ViewStack.SafeBindOnClearAction(stackOwnerObject, OnStackClear);
        }
        void OnStackOwnerDestroy()
        {
            if (!stackDestroyed)
            {
                OnStackClear();
            }
            Destroy(this);
            stackOwnerObject.OnDestroyEvent -= OnStackOwnerDestroy;
        }

        async void OnStackClear()
        {
            if (!stackDestroyed)
            {
                var stackViews = StackRoot.GetComponentsInChildren<IPresentableView>();
                var dismissTasks = Enumerable.Select(stackViews, presentableView => presentableView.Dismiss()).ToList();
                await dismissTasks;
                if (StackRoot)
                {
                    Destroy(StackRoot.gameObject);
                }
                stackDestroyed = true;
            }
        }

        async void OnActiveStackChange(ViewStackingUseCase activeViewStack)
        {
            if (ViewStack.Count > 0)
            {
                if (activeViewStack == this)
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
            }
            if (newView.View.GameObject != null)
            {
                if (formerTopView?.AttachParent)
                {
                    newView.View.Parent = formerTopView.AttachParent;   
                }
                await newView.Present();
            }
        }
        
        static async void OnPopView(IStackableViewModel poppedView, IStackableViewModel newTopView)
        {
            await poppedView.Dismiss();
            Destroy(poppedView.View.GameObject);
            if (newTopView != null)
            {
                await newTopView.Focus();
            }
        }
    }
}
