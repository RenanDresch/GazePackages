using Gaze.MVVM.Example.UseCases;
using Gaze.MVVM.ReadOnly;
using UnityEngine;

namespace Gaze.MVVM.Example.ViewModels
{
    [System.Serializable]
    public class ExampleViewModel : ViewModel
    {
        [SerializeField]
        WalletUseCase walletUseCase;

        [SerializeField]
        StoreUseCase storeUseCase;
        
        public uint HardCurrencyPackSize => storeUseCase.HardCurrencyPackSize;
        public uint SoftCurrencyPackSize => storeUseCase.SoftCurrencyPackSize;
        
        public IReactiveProperty<uint> HardCurrencyConsumption => storeUseCase.HardCurrencyCurrentConsumption;
        public IReactiveProperty<uint> SoftCurrencyConsumption => storeUseCase.SoftCurrencyCurrentConsumption;
        
        public IReactiveProperty<int> HardCurrencyAmount => walletUseCase.HardCurrencyAmount;
        public IReactiveProperty<int> SoftCurrencyAmount => walletUseCase.SoftCurrencyAmount;


        public void OnPressBuyUsingHardCurrency() => storeUseCase.BuyItemCurrencyUsingHardCurrency();

        public void OnPressBuyUsingSoftCurrency() => storeUseCase.BuyItemCurrencyUsingSoftCurrency();

        public void OnPressBuyHardCurrency() => storeUseCase.BuyHardCurrency();
        
        public void OnPressBuySoftCurrency() => storeUseCase.BuySoftCurrency();
    }
}
