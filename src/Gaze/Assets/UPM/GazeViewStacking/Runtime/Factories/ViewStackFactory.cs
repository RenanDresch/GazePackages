using Gaze.Utilities;
using UnityEngine;

namespace Gaze.MVVM.ViewStacking
{
    [System.Serializable]
    public class ViewStackFactory
    {
        [SerializeField]
        ViewStackingUseCase viewStackingUseCase;

        public ViewStackingUseCase GetViewStackingUseCase(IDestroyable stackObject)
        {
            var newUseCaseInstance = Object.Instantiate(viewStackingUseCase);
            newUseCaseInstance.SetupInstance(stackObject);
            return newUseCaseInstance;
        }
    }
}
