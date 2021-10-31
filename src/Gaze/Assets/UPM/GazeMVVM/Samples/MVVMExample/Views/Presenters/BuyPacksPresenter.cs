using System;
using Gaze.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Gaze.MVVM.Example.Views
{
    [Serializable]
    public class BuyPacksPresenter : ObservableMonoBehaviour
    {
        [SerializeField]
        Text hardCurrencyPackSizeLabel;
        [SerializeField]
        Text softCurrencyPackSizeLabel;

        [SerializeField]
        Button buyHardCurrencyButton;
        public Button BuyHardCurrencyButton => buyHardCurrencyButton;
        
        [SerializeField]
        Button buySoftCurrencyButton;
        public Button BuySoftCurrencyButton => buySoftCurrencyButton;
        
        public void Present(uint hardCurrencyPackSize, uint softCurrencyPackSize)
        {
            hardCurrencyPackSizeLabel.text = hardCurrencyPackSize.ToString();
            softCurrencyPackSizeLabel.text = softCurrencyPackSize.ToString();
        }
    }
}
