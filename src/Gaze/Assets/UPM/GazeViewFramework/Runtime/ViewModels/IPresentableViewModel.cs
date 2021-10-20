using Cysharp.Threading.Tasks;
using Gaze.MVVM.ReadOnly;

namespace Gaze.MVVM.ViewStacking
{
    public interface IPresentableViewModel : IViewModel
    {
        public UniTask Present();

        public UniTask Focus();
        public UniTask UnFocus();

        public UniTask Dismiss();
    }
}