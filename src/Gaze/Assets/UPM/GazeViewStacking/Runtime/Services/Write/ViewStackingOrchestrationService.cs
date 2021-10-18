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
        readonly IDestroyable destroyable;
        
        public ViewStackingOrchestrationService(IDestroyable destroyable, ViewStackServiceOrchestrationModel viewStackServiceOrchestrationModel)
        {
            this.destroyable = destroyable;
            this.viewStackServiceOrchestrationModel = viewStackServiceOrchestrationModel;
            SetupStackListeners();
        }

        void SetupStackListeners()
        {
            viewStackServiceOrchestrationModel.ViewStackingServices.SafeBindOnPushAction(destroyable, OnPushNewViewStackingService);
            viewStackServiceOrchestrationModel.ViewStackingServices.SafeBindOnPopAction(destroyable, OnPopViewStackingService);
        }
        
        void OnPushNewViewStackingService(IViewStackingService newStackingService, IViewStackingService formerTopStackingService)
        {
            if (formerTopStackingService != null)
            {
                formerTopStackingService.IsActiveStack.Value = false;
            }
            newStackingService.IsActiveStack.Value = true;
            newStackingService.ViewStack.SafeBindOnClearAction(destroyable, OnViewStackClear);
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
