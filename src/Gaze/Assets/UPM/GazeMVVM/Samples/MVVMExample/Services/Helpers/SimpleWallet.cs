using Gaze.MVVM.Example.Models;
using Gaze.Utilities;

namespace Gaze.MVVM.Example.Services
{
    public class SimpleWallet
    {
        readonly WalletModel wallet;
        
        public SimpleWallet(IDestroyable destroyable, BrokerModel broker, WalletModel wallet)
        {
            this.wallet = wallet;
            broker.Delta.SafeBindOnChangeAction(destroyable, OnCurrencyDelta);
        }
        
        void OnCurrencyDelta(int delta)
        {
            wallet.CurrencyAmount.Value += delta;
        }
    }
}
