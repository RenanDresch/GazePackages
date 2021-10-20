using Gaze.Utilities;
using UnityEngine;

namespace Gaze.MVVM.ViewStacking
{
    [System.Serializable]
    public class ViewStackFactory
    {
        [SerializeField]
        ViewStackingUseCase viewStackingUseCase;

        [SerializeField]
        GameObject viewStackCanvasPrefab;
        
        public ViewStackingUseCase GetViewStackingUseCase(IDestroyable stackOwnerObject)
        {
            var newStackCanvas = Object.Instantiate(viewStackCanvasPrefab).transform;
            var newUseCaseInstance = Object.Instantiate(viewStackingUseCase);
            newUseCaseInstance.SetupInstance(stackOwnerObject, newStackCanvas);
            return newUseCaseInstance;
        }
    }
}
