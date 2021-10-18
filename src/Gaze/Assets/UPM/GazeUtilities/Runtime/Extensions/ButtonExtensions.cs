using UnityEngine.Events;
using UnityEngine.UI;

namespace Gaze.Utilities
{
    public static class ButtonExtensions
    {
        public static void BindOnClick(this Button button, UnityAction action)
        {
            button.onClick.AddListener(action);
        }
        public static void UnbindOnClick(this Button button, UnityAction action)
        {
            button.onClick.RemoveListener(action);
        }
    }
}
