using Gaze.MCS.Example.Models;
using Gaze.Utilities;

namespace Gaze.MCS.Example.Services
{
    public class StoreService : BaseService
    {
        public StoreService(IDestroyable destroyable, Container container) : base(destroyable, container) {}
        
        public uint HardCurrencyPackSize => Store.HardCurrencyPackSize;
        public uint SoftCurrencyPackSize => Store.SoftCurrencyPackSize;

        public void BuyItemCurrencyUsingHardCurrency()
        {
            HardCurrencyBroker.Transaction.ForceUpdateValue(-(int)Store.HardCurrencyCurrentConsumption.Value);
            HardCurrencyBroker.TransitionSuccessful.SafeBindOnChangeAction(Destroyable, OnHardCurrencyTransactionComplete);
        }

        public void BuyItemCurrencyUsingSoftCurrency()
        {
            SoftCurrencyBroker.Transaction.ForceUpdateValue(-(int)Store.SoftCurrencyCurrentConsumption.Value);
            SoftCurrencyBroker.TransitionSuccessful.SafeBindOnChangeAction(Destroyable, OnSoftCurrencyTransactionComplete);
        }

        void OnHardCurrencyTransactionComplete(bool transactionSucceeded)
        {
            HardCurrencyBroker.TransitionSuccessful.UnbindOnChangeAction(OnHardCurrencyTransactionComplete);
            if (transactionSucceeded)
            {
                Store.HardCurrencyCurrentConsumption.Value += Store.HardCurrencyConsumptionIncrease;
            }
        }
        
        void OnSoftCurrencyTransactionComplete(bool transactionSucceeded)
        {
            SoftCurrencyBroker.TransitionSuccessful.UnbindOnChangeAction(OnSoftCurrencyTransactionComplete);
            if (transactionSucceeded)
            {
                Store.SoftCurrencyCurrentConsumption.Value += Store.SoftCurrencyConsumptionIncrease;
            }
        }
        
        public void BuyHardCurrency()
        {
            HardCurrencyBroker.Transaction.ForceUpdateValue((int)HardCurrencyPackSize);
        }
        
        public void BuySoftCurrency()
        {
            SoftCurrencyBroker.Transaction.ForceUpdateValue((int)HardCurrencyPackSize);
        }
    }
}
