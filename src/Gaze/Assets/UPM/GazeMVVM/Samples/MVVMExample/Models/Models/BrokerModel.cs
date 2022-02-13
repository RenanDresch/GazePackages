using System;
using Gaze.MVVM.ReadOnly;

namespace Gaze.MVVM.Example.Models
{
    public class BrokerModel
    {
        public readonly ReactiveProperty<Guid> TransactionID = new ReactiveProperty<Guid>();
        public IReactiveProperty<Guid> ReadOnlyTransactionID => TransactionID;

        public readonly ReactiveProperty<int> Transaction = new ReactiveProperty<int>();
        public IReactiveProperty<int> ReadOnlyTransaction => Transaction;
        
        public readonly ReactiveProperty<int> Delta = new ReactiveProperty<int>();
        public IReactiveProperty<int> ReadOnlyDelta => Delta;
        
        public readonly ReactiveProperty<bool> TransitionSuccessful = new ReactiveProperty<bool>();
        public IReactiveProperty<bool> ReadOnlySoftTransitionSuccessful => TransitionSuccessful;
    }
}
