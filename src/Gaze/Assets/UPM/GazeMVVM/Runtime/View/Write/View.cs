using System;
using Cysharp.Threading.Tasks;
using Gaze.MVVM.View.Read;
using Gaze.MVVM.ViewModel;
using Gaze.Utilities;
using UnityEngine;

namespace Source.Views
{
    [Serializable]
    public abstract class View<T> : ObservableMonoBehaviour, IView where T : ViewModel, new()
    {
        [SerializeField]
        T viewModel;

        public T ViewModel => viewModel;
        public GameObject GameObject => gameObject;
        public virtual Transform Parent
        {
            get => transform.parent;
            set => transform.SetParent(value, false);
        }

        protected override void Awake()
        {
            base.Awake();
            UniTask.Create(BootstrapView);
        }

        async UniTask BootstrapView()
        {
            try
            {
                await ViewModel.Setup(this);
            }
            catch (Exception e)
            {
                Debug.LogError(e, this);
                throw;
            }
        }

        public abstract UniTask Focus();

        public abstract UniTask UnFocus();
        
        public abstract UniTask Present();

        public abstract UniTask Dismiss();

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (viewModel == null)
            {
                viewModel = new T();
                viewModel.Reset();
            }
        }
#endif
    }
}
