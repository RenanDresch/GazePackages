using Cysharp.Threading.Tasks;

namespace Gaze.MVVM.ReadOnly.ViewStacking
{
    public interface IPresentableViewModel : IViewModel
    {
        public UniTask Present();

        public UniTask Focus();
        public UniTask UnFocus();

        public UniTask Dismiss();
    }
}