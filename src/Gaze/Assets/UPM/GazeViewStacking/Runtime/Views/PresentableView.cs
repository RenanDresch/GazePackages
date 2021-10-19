using System;
using Cysharp.Threading.Tasks;

namespace Gaze.MVVM.ViewStacking
{
    [Serializable]
    public abstract class PresentableView<T> : View<T>, IPresentableView where T : PresentableViewModel, new()
    {
        protected override void Awake()
        {
            base.Awake();
            ViewModel.SetupPresentableView(this);
        }

        public abstract UniTask Focus();

        public abstract UniTask UnFocus();
        
        public abstract UniTask Present();

        public abstract UniTask Dismiss();
    }
}
