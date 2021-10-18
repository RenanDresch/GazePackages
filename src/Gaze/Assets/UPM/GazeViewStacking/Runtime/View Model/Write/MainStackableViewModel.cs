using Cysharp.Threading.Tasks;
using Gaze.MVVM.View.Read;
using Gaze.ViewStacking.Services;

namespace Gaze.ViewStacking.ViewModel
{
    /// <summary>
    /// Responsible for orchestrating the ViewStack stack in the app, keep it as close to the app initialization as possible
    /// </summary>
    [System.Serializable]
    public abstract class MainStackableViewModel : RootStackableViewModel
    {
        ViewStackingOrchestrationService viewStackingOrchestrationService;
        public override async UniTask Setup(IView view)
        {
            viewStackingOrchestrationService = new ViewStackingOrchestrationService(view, viewStackServiceOrchestrationModel);
            await base.Setup(view);
        }
    }
}