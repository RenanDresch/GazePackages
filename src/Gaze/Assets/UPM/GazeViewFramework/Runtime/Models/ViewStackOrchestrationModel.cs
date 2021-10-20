using UnityEngine;

namespace Gaze.MVVM.ViewStacking
{
    [CreateAssetMenu(menuName = "View Stacking/Models/"+nameof(ViewStackOrchestrationModel))]
    public class ViewStackOrchestrationModel : ScriptableObject
    {
        public ReactiveProperty<ViewStackingUseCase> ActiveStackingService { get; } = new ReactiveProperty<ViewStackingUseCase>();
        public ReactiveStack<ViewStackingUseCase> ViewStackingServices { get; } = new ReactiveStack<ViewStackingUseCase>();
    }
}
