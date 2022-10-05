using Gaze.MCS.Example.Models;
using Gaze.Utilities;

namespace Gaze.MCS.Example.Services
{
    public class BrokerService : BaseService
    {
        readonly SimpleCurrencyBroker hardCurrencyBroker;
        readonly SimpleCurrencyBroker softCurrencyBroker;
        
        public BrokerService(IDestroyable destroyable, Container container) : base(destroyable, container)
        {
            hardCurrencyBroker = new SimpleCurrencyBroker(destroyable, HardCurrencyBroker, HardCurrencyWallet);
            softCurrencyBroker = new SimpleCurrencyBroker(destroyable, SoftCurrencyBroker, SoftCurrencyWallet);
        }
    }
}
