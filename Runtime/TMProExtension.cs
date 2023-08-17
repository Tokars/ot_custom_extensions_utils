using System;
using System.Linq;

namespace OT.Extensions
{
    public static class TMProExtension
    {
        public static void InitDp<T>(this TMPro.TMP_Dropdown dropdown, T[] items, Action<T> listener)
        {
            dropdown.ClearDp();
            dropdown.onValueChanged.AddListener(arg0 => listener?.Invoke(items[arg0]));
            dropdown.AddOptions(items.Select(i => i.ToString()).ToList());
            dropdown.interactable = true;
        }

        public static void ClearDp(this TMPro.TMP_Dropdown dropdown)
        {
            dropdown.onValueChanged.RemoveAllListeners();
            dropdown.options.Clear();
            dropdown.interactable = false;
        }
    }
}