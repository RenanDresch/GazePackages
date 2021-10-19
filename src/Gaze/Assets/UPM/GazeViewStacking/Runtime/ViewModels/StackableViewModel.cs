using Cysharp.Threading.Tasks;
using Gaze.MVVM.ReadOnly.ViewStacking;
using UnityEngine;

namespace Gaze.MVVM.ViewStacking
{
    [System.Serializable]
    public abstract class StackableViewModel : PresentableViewModel, IStackableViewModel
    {
        [SerializeField]
        protected ViewStackingOrchestrationUseCase viewStackingOrchestrationUseCase;
        
        [SerializeField]
        Transform attachParent;

        public Transform AttachParent => attachParent;

        public override async UniTask SetupPresentableView(IPresentableView presentableView)
        {
            await base.SetupPresentableView(presentableView);
            viewStackingOrchestrationUseCase.PushView(this);
        }
    }
}