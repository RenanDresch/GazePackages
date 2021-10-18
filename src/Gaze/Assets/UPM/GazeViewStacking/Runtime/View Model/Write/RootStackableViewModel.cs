using Cysharp.Threading.Tasks;
using Gaze.MVVM.View.Read;
using Gaze.ViewStacking.Services;
using UnityEngine;

namespace Gaze.ViewStacking.ViewModel
{
    /// <summary>
    /// Responsible for creating a new ViewStack 
    /// </summary>
    [System.Serializable]
    public abstract class RootStackableViewModel : StackableViewModel
    {
        public override async UniTask Setup(IView view)
        {
            var newStackParent = Object.Instantiate(viewStackServiceOrchestrationModel.ViewStackCanvasPrefab).transform;
            view.Parent = newStackParent;
            
            var newViewStackingService = new ViewStackingService(view, newStackParent);
            viewStackServiceOrchestrationModel.ViewStackingServices.Push(newViewStackingService);
            
            await base.Setup(view);
        }
    }
}