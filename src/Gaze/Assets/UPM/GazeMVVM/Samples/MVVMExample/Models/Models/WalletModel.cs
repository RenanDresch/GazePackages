using Gaze.MVVM.ReadOnly;

namespace Gaze.MVVM.Example.Models
{
    public class WalletModel
    {
        public readonly ReactiveProperty<int> CurrencyAmount = new ReactiveProperty<int>();
        public IReactiveProperty<int> ReadOnlyCurrencyAmount => CurrencyAmount;
    }
}
