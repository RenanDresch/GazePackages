using Gaze.MVVM.Example.Models;
using Gaze.Utilities;

namespace Gaze.MVVM.Example.Services
{
    public class StoreFeature
    {
        public readonly BrokerService BrokerService;
        public readonly WalletService WalletService;
        public readonly StoreService StoreService;

        public StoreFeature(IDestroyable destroyable, Container container)
        {
            BrokerService = new BrokerService(destroyable, container);
            WalletService = new WalletService(destroyable, container);
            StoreService = new StoreService(destroyable, container);
        }
    }
}
