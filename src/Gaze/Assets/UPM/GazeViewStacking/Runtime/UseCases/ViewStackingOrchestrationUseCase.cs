using Gaze.MVVM.ReadOnly.ViewStacking;
using Gaze.Utilities;
using UnityEngine;

namespace Gaze.MVVM.ViewStacking
{
    /// <summary>
    /// Orchestrate multiple View Stacking Services (the ViewStack stack)
    /// </summary>
    [CreateAssetMenu(menuName = "Use Cases/"+nameof(ViewStackingOrchestrationUseCase))]
    public class ViewStackingOrchestrationUseCase : ObservableScriptableObject
    {
        [SerializeField]
        ViewStackServiceOrchestrationModel viewStackServiceOrchestrationModel;

        protected override void OnEnable()
        {
            base.OnEnable();
            SetupStackListeners();
        }

        void SetupStackListeners()
        {
            viewStackServiceOrchestrationModel.ViewStackingServices.SafeBindOnPushAction(this, OnPushNewViewStackingService);
            viewStackServiceOrchestrationModel.ViewStackingServices.SafeBindOnPopAction(this, OnPopViewStackingService);
        }
        
        void OnPushNewViewStackingService(ViewStackingUseCase newStackingUseCase, ViewStackingUseCase formerTopStackingUseCase)
        {
            viewStackServiceOrchestrationModel.ActiveStackingService.Value = newStackingUseCase;
            newStackingUseCase.ViewStack.SafeBindOnClearAction(this, OnViewStackClear);
        }
        
        void OnPopViewStackingService(ViewStackingUseCase poppedViewStackingUseCase, ViewStackingUseCase newTopViewStackingUseCase)
        {
            viewStackServiceOrchestrationModel.ActiveStackingService.Value = newTopViewStackingUseCase;
        }
        
        void OnViewStackClear()
        {
            viewStackServiceOrchestrationModel.ViewStackingServices.Pop();
        }

        public void PushStack(ViewStackingUseCase viewStackingUseCase)
        {
            viewStackServiceOrchestrationModel.ViewStackingServices.Push(viewStackingUseCase);
        }
        
        public void PushView(IStackableViewModel stackableViewModel)
        {
            viewStackServiceOrchestrationModel.ActiveStackingService.Value.ViewStack.Push(stackableViewModel);
        }
        
        public void PopView()
        {
            if (viewStackServiceOrchestrationModel.ViewStackingServices.Count > 1 || viewStackServiceOrchestrationModel.ViewStackingServices.Peek().ViewStack.Count > 1)
            {
                viewStackServiceOrchestrationModel.ActiveStackingService.Value.ViewStack.Pop();
            }
        }
    }
}
