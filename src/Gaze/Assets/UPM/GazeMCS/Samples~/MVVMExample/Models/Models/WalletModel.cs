using Gaze.MCS.ReadOnly;

namespace Gaze.MCS.Example.Models
{
    public class WalletModel
    {
        public readonly ReactiveProperty<int> CurrencyAmount = new ReactiveProperty<int>();
        public IReactiveProperty<int> ReadOnlyCurrencyAmount => CurrencyAmount;
    }
}
