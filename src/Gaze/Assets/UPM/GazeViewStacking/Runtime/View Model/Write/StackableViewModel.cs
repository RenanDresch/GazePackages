using Cysharp.Threading.Tasks;
using Gaze.MVVM.View.Read;
using Gaze.ViewStacking.Model;
using Gaze.ViewStacking.ViewModel.Read;
using UnityEngine;

namespace Gaze.ViewStacking.ViewModel
{
    [System.Serializable]
    public abstract class StackableViewModel : MVVM.ViewModel.ViewModel, IStackableViewModel
    {
        [SerializeField]
        protected ViewStackServiceOrchestrationModel viewStackServiceOrchestrationModel;
        
        [SerializeField]
        Transform attachParent;

        public Transform AttachParent => attachParent;
        
        public override async UniTask Setup(IView view)
        {
            await base.Setup(view);
            viewStackServiceOrchestrationModel.ViewStackingServices.Peek().ViewStack.Push(this);
        }
    }
}