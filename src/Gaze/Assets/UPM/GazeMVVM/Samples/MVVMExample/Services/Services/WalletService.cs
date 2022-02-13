using Gaze.MVVM.Example.Models;
using Gaze.Utilities;

namespace Gaze.MVVM.Example.Services
{
    public class WalletService : BaseService
    {
        readonly SimpleWallet hardCurrencyWallet;
        readonly SimpleWallet softCurrencyWallet;
        
        public WalletService(IDestroyable destroyable, Container container) : base(destroyable, container)
        {
            hardCurrencyWallet = new SimpleWallet(destroyable, HardCurrencyBroker, HardCurrencyWallet);
            softCurrencyWallet = new SimpleWallet(destroyable, SoftCurrencyBroker, SoftCurrencyWallet);
        }
    }
}
