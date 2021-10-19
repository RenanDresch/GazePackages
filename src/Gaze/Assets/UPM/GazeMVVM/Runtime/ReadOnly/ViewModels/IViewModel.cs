using Cysharp.Threading.Tasks;

namespace Gaze.MVVM.ReadOnly
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
