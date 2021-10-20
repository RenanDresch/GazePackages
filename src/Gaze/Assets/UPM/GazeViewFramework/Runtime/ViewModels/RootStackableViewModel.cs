using Cysharp.Threading.Tasks;
using Gaze.MVVM.ReadOnly;
using UnityEngine;

namespace Gaze.MVVM.ViewStacking
{
    /// <summary>
    /// Responsible for creating a new ViewStack 
    /// </summary>
    [System.Serializable]
    public abstract class RootStackableViewModel : StackableViewModel
    {
        [SerializeField]
        ViewStackFactory viewStackFactory;
        
        public override async UniTask Setup(IView view)
        {
            var newViewStackingService = viewStackFactory.GetViewStackingUseCase(view);
            viewStackingOrchestrationUseCase.PushStack(newViewStackingService);
            
            view.Parent = newViewStackingService.StackRoot;
            
            await base.Setup(view);
        }
    }
}