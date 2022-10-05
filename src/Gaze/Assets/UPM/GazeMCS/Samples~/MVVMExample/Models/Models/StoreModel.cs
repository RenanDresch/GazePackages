using Gaze.MCS.ReadOnly;

namespace Gaze.MCS.Example.Models
{
    public class StoreModel
    {
        public readonly ReactiveProperty<uint> HardCurrencyCurrentConsumption = new ReactiveProperty<uint>(10);
        public IReactiveProperty<uint> ReadOnlyHardCurrencyCurrentConsumption => HardCurrencyCurrentConsumption;
        
        public readonly ReactiveProperty<uint> SoftCurrencyCurrentConsumption = new ReactiveProperty<uint>(10);
        public IReactiveProperty<uint> ReadOnlySoftCurrencyCurrentConsumption => SoftCurrencyCurrentConsumption;
       
        
        
        
        public uint HardCurrencyConsumptionIncrease => 2;
        public uint SoftCurrencyConsumptionIncrease => 4;
        public uint HardCurrencyPackSize => 100;
        public uint SoftCurrencyPackSize => 200;
    }
}
