using Cysharp.Threading.Tasks;
using Gaze.MVVM.View.Read;

namespace Gaze.MVVM.ViewModel.Read
{
    public interface IViewModel
    {
        public IView View { get; } 
        public UniTask Present();

        public UniTask Focus();
        public UniTask UnFocus();

        public UniTask Dismiss();
    }
}
