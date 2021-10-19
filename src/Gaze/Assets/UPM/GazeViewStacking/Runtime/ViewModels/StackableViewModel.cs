using Cysharp.Threading.Tasks;
using Gaze.MVVM.ReadOnly;
using Gaze.MVVM.ReadOnly.ViewStacking;
using UnityEngine;

namespace Gaze.MVVM.ViewStacking
{
    [System.Serializable]
    public abstract class StackableViewModel : ViewModel, IStackableViewModel
    {
        [SerializeField]
        protected ViewStackingOrchestrationUseCase viewStackingOrchestrationUseCase;
        
        [SerializeField]
        Transform attachParent;

        public Transform AttachParent => attachParent;
        
        public override async UniTask Setup(IView view)
        {
            await base.Setup(view);
            viewStackingOrchestrationUseCase.PushView(this);
        }
    }
}