using UnityEngine;

namespace Gaze.MVVM.Example.Models
{
    [CreateAssetMenu(menuName = "Samples/MVVM/Models/"+nameof(WalletModel))]
    public class WalletModel : ScriptableObject
    {
        public ReactiveProperty<int> SoftCurrencyAmount { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<int> HardCurrencyAmount { get; } = new ReactiveProperty<int>();
    }
}
