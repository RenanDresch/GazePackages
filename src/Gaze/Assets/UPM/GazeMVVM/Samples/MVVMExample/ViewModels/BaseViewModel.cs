using System;
using Gaze.MVVM.Example.Models;
using UnityEngine;

namespace Gaze.MVVM.Example.ViewModels
{
    [Serializable]
    public class BaseViewModel : ViewModel
    {
        [SerializeField]
        Container container;
        public Container Container => container;
        public StoreModel Store => container.StoreModel;
        public WalletModel HardCurrencyWallet => container.HardCurrencyWalletModel;
        public WalletModel SoftCurrencyWallet => container.SoftCurrencyWalletModel;
    }
}
