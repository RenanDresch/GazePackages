using Gaze.MVVM.Example.Services;
using Gaze.MVVM.ReadOnly;

namespace Gaze.MVVM.Example.ViewModels
{
    [System.Serializable]
    public class ExampleViewModel : BaseViewModel
    {
        StoreFeature storeFeature;
        StoreService StoreService => storeFeature.StoreService;
        
        public uint HardCurrencyPackSize => StoreService.HardCurrencyPackSize;
        public uint SoftCurrencyPackSize => StoreService.SoftCurrencyPackSize;

        public override void OnStart(IView view)
        {
            base.OnStart(view);
            storeFeature = new StoreFeature(view, Container);
        }

        public IReactiveProperty<uint> HardCurrencyConsumption => Store.ReadOnlyHardCurrencyCurrentConsumption;
        public IReactiveProperty<uint> SoftCurrencyConsumption => Store.ReadOnlySoftCurrencyCurrentConsumption;

        public IReactiveProperty<int> HardCurrencyAmount => HardCurrencyWallet.ReadOnlyCurrencyAmount;
        public IReactiveProperty<int> SoftCurrencyAmount => SoftCurrencyWallet.ReadOnlyCurrencyAmount;
        

        public void OnPressBuyUsingHardCurrency() => StoreService.BuyItemCurrencyUsingHardCurrency();
        
        public void OnPressBuyUsingSoftCurrency() => StoreService.BuyItemCurrencyUsingSoftCurrency();
        
        public void OnPressBuyHardCurrency() => StoreService.BuyHardCurrency();
        
        public void OnPressBuySoftCurrency() => storeFeature.StoreService.BuySoftCurrency();
    }
}
