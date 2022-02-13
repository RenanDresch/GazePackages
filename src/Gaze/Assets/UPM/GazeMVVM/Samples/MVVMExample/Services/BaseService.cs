using Gaze.MVVM.Example.Models;
using Gaze.MVVM.Services;
using Gaze.Utilities;

namespace Gaze.MVVM.Example.Services
{
    public class BaseService : Service
    {
        readonly Container container;

        protected BrokerModel SoftCurrencyBroker => container.SoftCurrencyBrokerModel;
        protected BrokerModel HardCurrencyBroker => container.HardCurrencyBrokerModel;
        protected WalletModel SoftCurrencyWallet => container.SoftCurrencyWalletModel;
        protected WalletModel HardCurrencyWallet => container.HardCurrencyWalletModel;
        protected StoreModel Store => container.StoreModel;

        public BaseService(IDestroyable destroyable, Container container) : base(destroyable)
        {
            this.container = container;
        }
    }
}
