using Gaze.Utilities;
using Gaze.ViewStacking.Model;
using Gaze.ViewStacking.Services.Read;

namespace Gaze.ViewStacking.Services
{
    /// <summary>
    /// Orchestrate multiple View Stacking Services (the ViewStack stack)
    /// </summary>
    public class ViewStackingOrchestrationService
    {
        readonly ViewStackServiceOrchestrationModel viewStackServiceOrchestrationModel;
        readonly IObservableMonoBehaviour observableMonoBehaviour;
        
        public ViewStackingOrchestrationService(IObservableMonoBehaviour observableMonoBehaviour, ViewStackServiceOrchestrationModel viewStackServiceOrchestrationModel)
        {
            this.observableMonoBehaviour = observableMonoBehaviour;
            this.viewStackServiceOrchestrationModel = viewStackServiceOrchestrationModel;
            SetupStackListeners();
        }

        void SetupStackListeners()
        {
            viewStackServiceOrchestrationModel.ViewStackingServices.SafeBindOnPushAction(observableMonoBehaviour, OnPushNewViewStackingService);
            viewStackServiceOrchestrationModel.ViewStackingServices.SafeBindOnPopAction(observableMonoBehaviour, OnPopViewStackingService);
        }
        
        void OnPushNewViewStackingService(IViewStackingService newStackingService, IViewStackingService formerTopStackingService)
        {
            if (formerTopStackingService != null)
            {
                formerTopStackingService.IsActiveStack.Value = false;
            }
            newStackingService.IsActiveStack.Value = true;
            newStackingService.ViewStack.SafeBindOnClearAction(observableMonoBehaviour, OnViewStackClear);
        }
        
        void OnPopViewStackingService(IViewStackingService poppedViewStackingService, IViewStackingService newTopViewStackingService)
        {
            newTopViewStackingService.IsActiveStack.Value = true;
        }
        
        void OnViewStackClear()
        {
            viewStackServiceOrchestrationModel.ViewStackingServices.Pop();
        }
    }
}
