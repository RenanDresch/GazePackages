using Cysharp.Threading.Tasks;
using Gaze.Utilities;
using UnityEngine;

namespace Gaze.MVVM.View.Read
{
    public interface IView : IDestroyable
    {
        GameObject GameObject { get; }
        Transform Parent { get; set; }

        UniTask Focus();
        UniTask UnFocus();
        UniTask Present();
        UniTask Dismiss();
    }
}
