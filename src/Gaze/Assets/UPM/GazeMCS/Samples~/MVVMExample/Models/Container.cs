using UnityEngine;

namespace Gaze.MCS.Example.Models
{
    [CreateAssetMenu(menuName = "Samples/MVVM/Models/"+nameof(Container))]
    public class Container : ScriptableObject
    {
        public readonly BrokerModel SoftCurrencyBrokerModel = new BrokerModel();
        public readonly BrokerModel HardCurrencyBrokerModel = new BrokerModel();
        
        public readonly WalletModel SoftCurrencyWalletModel = new WalletModel();
        public readonly WalletModel HardCurrencyWalletModel = new WalletModel();
        
        public readonly StoreModel StoreModel = new StoreModel();
    }
}
