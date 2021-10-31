using Gaze.MVVM.Example.Services;
using Gaze.MVVM.ReadOnly;
using UnityEngine;

namespace Gaze.MVVM.Example.ViewModels
{
    [System.Serializable]
    public class ExampleViewModel : ViewModel
    {
        [SerializeField]
        WalletService walletService;

        [SerializeField]
        StoreService storeService;
        
        public uint HardCurrencyPackSize => storeService.HardCurrencyPackSize;
        public uint SoftCurrencyPackSize => storeService.SoftCurrencyPackSize;
        
        public IReactiveProperty<uint> HardCurrencyConsumption => storeService.HardCurrencyCurrentConsumption;
        public IReactiveProperty<uint> SoftCurrencyConsumption => storeService.SoftCurrencyCurrentConsumption;
        
        public IReactiveProperty<int> HardCurrencyAmount => walletService.HardCurrencyAmount;
        public IReactiveProperty<int> SoftCurrencyAmount => walletService.SoftCurrencyAmount;


        public void OnPressBuyUsingHardCurrency() => storeService.BuyItemCurrencyUsingHardCurrency();

        public void OnPressBuyUsingSoftCurrency() => storeService.BuyItemCurrencyUsingSoftCurrency();

        public void OnPressBuyHardCurrency() => storeService.BuyHardCurrency();
        
        public void OnPressBuySoftCurrency() => storeService.BuySoftCurrency();
    }
}
