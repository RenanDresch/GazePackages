using Cysharp.Threading.Tasks;
using Gaze.MVVM.ReadOnly;

namespace Gaze.MVVM.ViewStacking
{
    public interface IPresentableView : IView
    {
        UniTask Focus();
        UniTask UnFocus();
        UniTask Present();
        UniTask Dismiss();
    }
}
