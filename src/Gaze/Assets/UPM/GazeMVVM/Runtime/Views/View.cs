using System;
using Gaze.MVVM.ReadOnly;
using Gaze.Utilities;
using UnityEngine;

namespace Gaze.MVVM
{
    [Serializable]
    public abstract class View<T> : ObservableMonoBehaviour, IView where T : ViewModel, new()
    {
        [SerializeField]
        T viewModel;

        public T ViewModel => viewModel;
        public GameObject GameObject => this ? gameObject : null;
        public virtual Transform Parent
        {
            get => transform.parent;
            set => transform.SetParent(value, false);
        }

        protected override void Awake()
        {
            base.Awake();
            ViewModel.OnStart(this);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            viewModel.OnDestroy();
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (viewModel == null)
            {
                viewModel = new T();
                viewModel.Reset();
            }
            viewModel.OnValidate();
        }
#endif
    }
}
