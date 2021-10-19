using UnityEngine;

namespace Gaze.MVVM.ReadOnly.ViewStacking
{
    public interface IStackableViewModel : IViewModel
    {
        public Transform AttachParent { get; }
    }
}