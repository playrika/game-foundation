using System.Collections.Generic;
using System.Linq;
using Playrika.GameFoundation.Common;
using Playrika.GameFoundation.Helpers;
using UnityEngine;

namespace Playrika.GameFoundation.UI
{
    public class WindowManager : Singleton<WindowManager>
    {
        private List<WindowBase> windows { get; } = new List<WindowBase>();

        private string _resourcesPath = "UI/Windows";

        private int _sortingOrder;


        public static void SetResourcesPath(string value)
        {
            instance._resourcesPath = value;
        }

        public static T Show<T>() where T : WindowBase
        {
            instance._sortingOrder++;

            var existWindow = instance.windows.FirstOrDefault(w => w is T);
            if (existWindow != null)
            {
                existWindow.canvas.sortingOrder = instance._sortingOrder;
                existWindow.transform.SetAsLastSibling();
                return existWindow as T;
            }

            var type = typeof(T);
            var deviceType = DeviceHelper.deviceType;

            var windowPrefab = Resources.Load<T>($"{instance._resourcesPath}/{type.Name}_{deviceType}");
            if (windowPrefab == null)
                windowPrefab = Resources.Load<T>($"{instance._resourcesPath}/{type.Name}");

            var window = Instantiate(windowPrefab, instance.transform, false);
            window.name = type.Name;

            window.canvas.sortingOrder = instance._sortingOrder;
            instance.windows.Add(window);

            return window;
        }

        public static T Get<T>() where T : WindowBase
        {
            var existWindow = instance.windows.FirstOrDefault(w => w is T);
            if (existWindow == null)
                return default;

            return existWindow as T;
        }

        public static void Close(WindowBase window)
        {
            var lastWindow = instance.windows.LastOrDefault();
            if (lastWindow == window)
                instance._sortingOrder--;

            DestroyWindow(window);
        }

        public static void Close<T>() where T : WindowBase
        {
            var lastWindow = instance.windows.LastOrDefault();
            if (lastWindow is T)
            {
                instance._sortingOrder--;
                DestroyWindow(lastWindow);
                return;
            }

            DestroyWindow(instance.windows.LastOrDefault(w => w is T));
        }

        public static void CloseAll()
        {
            foreach (var window in instance.windows)
                Destroy(window.gameObject);

            instance.windows.Clear();
            instance._sortingOrder = 0;
        }


        private static void DestroyWindow(WindowBase window)
        {
            if (window == null)
                return;

            instance.windows.Remove(window);
            Destroy(window.gameObject);

            if (instance.windows.Count <= 0)
                instance._sortingOrder = 0;
        }
    }
}