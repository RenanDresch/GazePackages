using UnityEngine;

namespace Gaze.MVVM.ViewStacking
{
    [CreateAssetMenu(menuName = "View Stacking/Models/"+nameof(ViewStackOrchestrationModel))]
    public class ViewStackOrchestrationModel : ScriptableObject
    {
        public ReactiveProperty<ViewStackingService> ActiveStackingService { get; } = new ReactiveProperty<ViewStackingService>();
        public ReactiveStack<ViewStackingService> ViewStackingServices { get; } = new ReactiveStack<ViewStackingService>();
    }
}
