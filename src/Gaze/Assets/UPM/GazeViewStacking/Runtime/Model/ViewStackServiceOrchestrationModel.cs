using Gaze.MVVM.Model;
using Gaze.ViewStacking.Services.Read;
using UnityEngine;

namespace Gaze.ViewStacking.Model
{
    [CreateAssetMenu(menuName = "View Stacking/Models/"+nameof(ViewStackServiceOrchestrationModel))]
    public class ViewStackServiceOrchestrationModel : ScriptableObject
    {
        [SerializeField]
        GameObject viewStackCanvasPrefab;
        public GameObject ViewStackCanvasPrefab => viewStackCanvasPrefab;
        
        [SerializeField]
        ReactiveStack<IViewStackingService> viewStackingServices = new ReactiveStack<IViewStackingService>();
        public ReactiveStack<IViewStackingService> ViewStackingServices => viewStackingServices;
    }
}
