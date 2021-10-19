using UnityEngine;

namespace Gaze.MVVM.ReadOnly.ViewStacking
{
    public interface IStackableViewModel : IPresentableViewModel
    {
        public Transform AttachParent { get; }
    }
}