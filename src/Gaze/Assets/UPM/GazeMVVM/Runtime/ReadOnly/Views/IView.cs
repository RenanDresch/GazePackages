using Gaze.Utilities;
using UnityEngine;

namespace Gaze.MVVM.ReadOnly
{
    public interface IView : IDestroyable
    {
        GameObject GameObject { get; }
        Transform Parent { get; set; }
    }
}
