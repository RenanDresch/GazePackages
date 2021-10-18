using Gaze.MVVM.ViewModel.Read;
using UnityEngine;

namespace Gaze.ViewStacking.ViewModel.Read
{
    public interface IStackableViewModel : IViewModel
    {
        public Transform AttachParent { get; }
    }
}