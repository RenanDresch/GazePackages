using Gaze.Utilities;
using UnityEngine;

namespace Gaze.MVVM.ViewStacking
{
    [System.Serializable]
    public class ViewStackFactory
    {
        [SerializeField]
        ViewStackingService viewStackingService;

        [SerializeField]
        GameObject viewStackCanvasPrefab;
        
        public ViewStackingService GetViewStackingUseCase(IDestroyable stackOwnerObject)
        {
            var newStackCanvas = Object.Instantiate(viewStackCanvasPrefab).transform;
            var newUseCaseInstance = Object.Instantiate(viewStackingService);
            newUseCaseInstance.SetupInstance(stackOwnerObject, newStackCanvas);
            return newUseCaseInstance;
        }
    }
}
