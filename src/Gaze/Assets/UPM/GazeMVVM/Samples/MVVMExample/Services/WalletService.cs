using Gaze.MVVM.Example.Models;
using Gaze.MVVM.ReadOnly;
using Gaze.Utilities;
using UnityEngine;

namespace Gaze.MVVM.Example.Services
{
    [CreateAssetMenu(menuName = "Samples/MVVM/Services/"+nameof(WalletService))]
    public class WalletService : ObservableScriptableObject
    {
        [SerializeField]
        WalletModel walletModel;

        public IReactiveProperty<int> HardCurrencyAmount => walletModel.HardCurrencyAmount;
        public IReactiveProperty<int> SoftCurrencyAmount => walletModel.SoftCurrencyAmount;

        internal void AddHardCurrency(uint amount)
        {
            walletModel.HardCurrencyAmount.Value += (int)amount;
        }
        
        internal bool ConsumeHardCurrency(uint amount)
        {
            var hasEnoughCurrency = walletModel.HardCurrencyAmount >= (int)amount;
            if (hasEnoughCurrency)
            {
                walletModel.HardCurrencyAmount.Value -= (int)amount;
            }
            return hasEnoughCurrency;
        }
        
        internal void AddSoftCurrency(uint amount)
        {
            walletModel.SoftCurrencyAmount.Value += (int)amount;
        }
        
        internal bool ConsumeSoftCurrency(uint amount)
        {
            var hasEnoughCurrency = walletModel.SoftCurrencyAmount >= (int)amount;
            if (hasEnoughCurrency)
            {
                walletModel.SoftCurrencyAmount.Value -= (int)amount;
            }
            return hasEnoughCurrency;
        }
    }
}
