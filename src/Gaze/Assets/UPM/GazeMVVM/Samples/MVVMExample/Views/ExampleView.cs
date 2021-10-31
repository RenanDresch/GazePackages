using Gaze.MVVM.Example.ViewModels;
using Gaze.Utilities;
using UnityEngine;

namespace Gaze.MVVM.Example.Views
{
    public class ExampleView : View<ExampleViewModel>
    {
        [SerializeField]
        WalletPresenter walletPresenter;
        
        [SerializeField]
        BuyPacksPresenter buyPacksPresenter;
        
        [SerializeField]
        BuyItemsPresenter buyItemsPresenter;

        protected override void Start()
        {
            base.Start();
            walletPresenter.Present(ViewModel.HardCurrencyAmount, ViewModel.SoftCurrencyAmount);
            buyPacksPresenter.Present(ViewModel.HardCurrencyPackSize, ViewModel.SoftCurrencyPackSize);
            buyItemsPresenter.Present(ViewModel.HardCurrencyConsumption, ViewModel.SoftCurrencyConsumption);
            
            buyPacksPresenter.BuyHardCurrencyButton.BindOnClick(ViewModel.OnPressBuyHardCurrency);
            buyPacksPresenter.BuySoftCurrencyButton.BindOnClick(ViewModel.OnPressBuySoftCurrency);
            
            buyItemsPresenter.BuyUsingHardCurrencyButton.BindOnClick(ViewModel.OnPressBuyUsingHardCurrency);
            buyItemsPresenter.BuyUsingSoftCurrencyButton.BindOnClick(ViewModel.OnPressBuyUsingSoftCurrency);
        }
    }
}
