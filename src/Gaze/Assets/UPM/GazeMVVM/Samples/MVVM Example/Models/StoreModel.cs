using UnityEngine;

namespace Gaze.MVVM.Example.Models
{
    [CreateAssetMenu(menuName = "Samples/MVVM/Models/"+nameof(StoreModel))]
    public class StoreModel : ScriptableObject
    {
        [SerializeField]
        int startingHardCurrencyConsumption = 10;
        [SerializeField]
        int startingSoftCurrencyConsumption = 10;

        [SerializeField]
        int hardCurrencyConsumptionIncrease = 2;
        public uint HardCurrencyConsumptionIncrease => (uint)hardCurrencyConsumptionIncrease;
        
        [SerializeField]
        int softCurrencyConsumptionIncrease = 4;
        public uint SoftCurrencyConsumptionIncrease => (uint)softCurrencyConsumptionIncrease;
        
        [SerializeField]
        int hardCurrencyPackSize = 100;
        public uint HardCurrencyPackSize => (uint)hardCurrencyPackSize;
        
        [SerializeField]
        int softCurrencyPackSize = 200;
        public uint SoftCurrencyPackSize => (uint)softCurrencyPackSize;
        
        public ReactiveProperty<uint> HardCurrencyCurrentConsumption { get; private set; }
        public ReactiveProperty<uint> SoftCurrencyCurrentConsumption { get; private set; }
     
        void OnEnable()
        {
            HardCurrencyCurrentConsumption = new ReactiveProperty<uint>((uint)startingHardCurrencyConsumption);
            SoftCurrencyCurrentConsumption = new ReactiveProperty<uint>((uint)startingSoftCurrencyConsumption);
        }
    }
}
