using Gaze.MVVM.Model;
using Gaze.ViewStacking.ViewModel.Read;

namespace Gaze.ViewStacking.Services.Read
{
    public interface IViewStackingService
    {
        public ReactiveProperty<bool> IsActiveStack { get; }
        public ReactiveStack<IStackableViewModel> ViewStack { get; }
    }
}
