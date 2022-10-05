using System;
using Gaze.MCS.Example.Models;
using Gaze.Utilities;
using UnityEngine;

namespace Gaze.MCS.Example.Services
{
    public class SimpleCurrencyBroker
    {
        readonly IDestroyable destroyable;
        readonly BrokerModel broker;
        readonly WalletModel wallet;
        
        public SimpleCurrencyBroker(IDestroyable destroyable, BrokerModel broker, WalletModel wallet)
        {
            this.destroyable = destroyable;
            this.broker = broker;
            this.wallet = wallet;

            broker.Transaction.SafeBindOnChangeAction(destroyable, OnTransaction);
        }
        
        void OnTransaction(int transaction)
        {
            broker.TransactionID.ForceUpdateValue(Guid.NewGuid());
            if (transaction > 0 || TransactionIsValid(transaction, wallet.CurrencyAmount))
            {
                SucceedTransaction(transaction);
            }
            else
            {
                if (transaction == 0)
                {
                    Debug.LogWarning($"Attempting to execute a 0 value transaction! | Transaction Id {broker.TransactionID.Value}");
                }
                broker.TransitionSuccessful.ForceUpdateValue(false);
            }
        }

        static bool TransactionIsValid(int transaction, int availableAmount) => -transaction <= availableAmount;

        void SucceedTransaction(int transaction)
        {
            broker.Delta.ForceUpdateValue(transaction);
            broker.TransitionSuccessful.ForceUpdateValue(true);
        }
    }
}
