using UnityEngine;

namespace Gaze.MVVM.ViewStacking
{
    [CreateAssetMenu(menuName = "View Stacking/Models/"+nameof(ViewStackServiceOrchestrationModel))]
    public class ViewStackServiceOrchestrationModel : ScriptableObject
    {
        [SerializeField]
        GameObject viewStackCanvasPrefab;
        public GameObject ViewStackCanvasPrefab => viewStackCanvasPrefab;

        [SerializeField]
        ReactiveProperty<ViewStackingUseCase> activeStackingService = new ReactiveProperty<ViewStackingUseCase>();
        public ReactiveProperty<ViewStackingUseCase> ActiveStackingService => activeStackingService;
        
        [SerializeField]
        ReactiveStack<ViewStackingUseCase> viewStackingServices = new ReactiveStack<ViewStackingUseCase>();
        public ReactiveStack<ViewStackingUseCase> ViewStackingServices => viewStackingServices;
    }
}
