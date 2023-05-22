using System;
using Gaze.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gaze.MCS
{
    public class Test2 : MonoBehaviour, IDestroyable
    {
        IReactiveProperty<int> random = new ReactiveProperty<int>();

        void Update()
        {
            random.Value = Random.Range(0, 999);
            if (Input.anyKeyDown)
            {
                random.SafeBindOnChangeAction(this, OnChange);
                random.SafeBindOnReplaceAction(this, OnReplace);
            }
        }

        void OnReplace((int oldValue, int newValue) obj)
        {
        }

        void OnChange(int value)
        {
        }

        public event Action OnDestroyEvent;
        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
