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
        
        void OnPushNewViewStackingService(ViewStackingUseCase newStackingUseCase, ViewStackingUseCase formerTopStackingUseCase)
        {
            viewStackOrchestrationModel.ActiveStackingService.Value = newStackingUseCase;
            newStackingUseCase.ViewStack.SafeBindOnClearAction(this, OnViewStackClear);
        }
        
        void OnPopViewStackingService(ViewStackingUseCase poppedViewStackingUseCase, ViewStackingUseCase newTopViewStackingUseCase)
        {
            viewStackOrchestrationModel.ActiveStackingService.Value = newTopViewStackingUseCase;
        }
        
        void OnViewStackClear()
        {
            viewStackOrchestrationModel.ViewStackingServices.Pop();
        }

        public void PushStack(ViewStackingUseCase viewStackingUseCase)
        {
            viewStackOrchestrationModel.ViewStackingServices.Push(viewStackingUseCase);
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
