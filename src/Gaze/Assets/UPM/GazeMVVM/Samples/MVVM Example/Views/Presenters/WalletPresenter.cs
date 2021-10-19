using System;
using Gaze.MVVM.ReadOnly;
using Gaze.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Gaze.MVVM.Example.Views
{
    [Serializable]
    public class WalletPresenter : ObservableMonoBehaviour
    {
        [SerializeField]
        Text hardCurrencyAmountLabel;
        [SerializeField]
        Text softCurrencyAmountLabel;

        public void Present(IReactiveProperty<int> hardCurrencyAmount, IReactiveProperty<int> softCurrencyAmount)
        {
            hardCurrencyAmount.SafeBindOnChangeAction(this, (value) => hardCurrencyAmountLabel.text = value.ToString());
            softCurrencyAmount.SafeBindOnChangeAction(this, (value) => softCurrencyAmountLabel.text = value.ToString());
        }
    }
}
