using System;
using Gaze.MVVM.ReadOnly;
using Gaze.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Gaze.MVVM.Example.Views
{
    [Serializable]
    public class BuyItemsPresenter : ObservableMonoBehaviour
    {
        [SerializeField]
        Text hardCurrencyCostLabel;
        [SerializeField]
        Text softCurrencyCostLabel;

        [SerializeField]
        Button buyUsingHardCurrencyButton;
        public Button BuyUsingHardCurrencyButton => buyUsingHardCurrencyButton;
        
        [SerializeField]
        Button buyUsingSoftCurrencyButton;
        public Button BuyUsingSoftCurrencyButton => buyUsingSoftCurrencyButton;
        
        public void Present(IReactiveProperty<uint> hardCurrencyCost, IReactiveProperty<uint> softCurrencyCost)
        {
            hardCurrencyCost.SafeBindOnChangeAction(this, (value) => hardCurrencyCostLabel.text = value.ToString());
            softCurrencyCost.SafeBindOnChangeAction(this, (value) => softCurrencyCostLabel.text = value.ToString());
        }
    }
}
