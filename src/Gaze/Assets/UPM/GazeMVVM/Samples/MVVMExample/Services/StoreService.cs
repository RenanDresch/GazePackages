using Gaze.MVVM.Example.Models;
using Gaze.MVVM.ReadOnly;
using Gaze.Utilities;
using UnityEngine;

namespace Gaze.MVVM.Example.Services
{
    [CreateAssetMenu(menuName = "Samples/MVVM/Services/"+nameof(StoreService))]
    public class StoreService : ObservableScriptableObject
    {
        [SerializeField]
        StoreModel storeModel;
        
        [SerializeField]
        WalletService walletService;

        public uint HardCurrencyPackSize => storeModel.HardCurrencyPackSize;
        public uint SoftCurrencyPackSize => storeModel.SoftCurrencyPackSize;
        public IReactiveProperty<uint> HardCurrencyCurrentConsumption => storeModel.HardCurrencyCurrentConsumption;
        public IReactiveProperty<uint> SoftCurrencyCurrentConsumption => storeModel.SoftCurrencyCurrentConsumption;

        public void BuyItemCurrencyUsingHardCurrency()
        {
            if (walletService.ConsumeHardCurrency(storeModel.HardCurrencyCurrentConsumption))
            {
                storeModel.HardCurrencyCurrentConsumption.Value += storeModel.HardCurrencyConsumptionIncrease;
            }
        }
        
        public void BuyItemCurrencyUsingSoftCurrency()
        {
            if (walletService.ConsumeSoftCurrency(storeModel.HardCurrencyCurrentConsumption))
            {
                storeModel.SoftCurrencyCurrentConsumption.Value += storeModel.SoftCurrencyConsumptionIncrease;
            }
        }

        public void BuyHardCurrency()
        {
            walletService.AddHardCurrency(HardCurrencyPackSize);
        }
        
        public void BuySoftCurrency()
        {
            walletService.AddSoftCurrency(SoftCurrencyPackSize);
        }
    }
}
