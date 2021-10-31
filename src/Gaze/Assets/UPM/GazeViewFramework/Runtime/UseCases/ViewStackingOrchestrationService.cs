using Gaze.MVVM.ReadOnly.ViewStacking;
using Gaze.Utilities;
using UnityEngine;

namespace Gaze.MVVM.ViewStacking
{
    /// <summary>
    /// Orchestrate multiple View Stacking Services (the ViewStack stack)
    /// </summary>
    [CreateAssetMenu(menuName = "MVVM/ViewFramework/Services/"+nameof(ViewStackingOrchestrationService))]
    public class ViewStackingOrchestrationService : ObservableScriptableObject
    {
        [SerializeField]
        ViewStackOrchestrationModel viewStackOrchestrationModel;

        protected override void OnEnable()
        {
            base.OnEnable();
            SetupStackListeners();
        }

        void SetupStackListeners()
        {
            viewStackOrchestrationModel.ViewStackingServices.SafeBindOnPushAction(this, OnPushNewViewStackingService);
            viewStackOrchestrationModel.ViewStackingServices.SafeBindOnPopAction(this, OnPopViewStackingService);
        }
        
        void OnPushNewViewStackingService(ViewStackingService newStackingService, ViewStackingService formerTopStackingService)
        {
            viewStackOrchestrationModel.ActiveStackingService.Value = newStackingService;
            newStackingService.ViewStack.SafeBindOnClearAction(this, OnViewStackClear);
        }
        
        void OnPopViewStackingService(ViewStackingService poppedViewStackingService, ViewStackingService newTopViewStackingService)
        {
            viewStackOrchestrationModel.ActiveStackingService.Value = newTopViewStackingService;
        }
        
        void OnViewStackClear()
        {
            viewStackOrchestrationModel.ViewStackingServices.Pop();
        }

        public void PushStack(ViewStackingService viewStackingService)
        {
            viewStackOrchestrationModel.ViewStackingServices.Push(viewStackingService);
        }
        
        public void PushView(IStackableViewModel stackableViewModel)
        {
            viewStackOrchestrationModel.ActiveStackingService.Value.ViewStack.Push(stackableViewModel);
        }
        
        public void PopView()
        {
            if (viewStackOrchestrationModel.ViewStackingServices.Count > 1 || viewStackOrchestrationModel.ViewStackingServices.Peek().ViewStack.Count > 1)
            {
                viewStackOrchestrationModel.ActiveStackingService.Value.ViewStack.Pop();
            }
        }
    }
}
