using System;
using System.Threading;
using Cysharp.Threading.Tasks;
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
        
        readonly CancellationTokenSource destructionCancellationTokenSource = new CancellationTokenSource();
        protected CancellationToken DestructionCancellationToken => destructionCancellationTokenSource.Token;
        
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
            OnDestroyEvent += () =>
            {
                destructionCancellationTokenSource.Cancel();
                destructionCancellationTokenSource.Dispose();
            };
            UniTask.Create(BootstrapView).AttachExternalCancellation(destructionCancellationTokenSource.Token).SuppressCancellationThrow().Forget();
        }

        async UniTask BootstrapView()
        {
            await ViewModel.OnStart(this);
        }

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
