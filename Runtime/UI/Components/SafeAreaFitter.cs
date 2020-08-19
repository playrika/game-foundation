using UnityEngine;

namespace Playrika.GameFoundation.UI.Components
{
    [RequireComponent(typeof(RectTransform)), AddComponentMenu("GameFoundation/UI/Components/SafeAreaFitter")]
    public class SafeAreaFitter : MonoBehaviour
    {
        [SerializeField] private bool _fitOnAwake;

        public void Fit()
        {
            var rectTransform = GetComponent<RectTransform>();
            var safeArea = Screen.safeArea;
            var anchorMin = safeArea.position;
            var anchorMax = safeArea.position + safeArea.size;

            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
        }

        private void Awake()
        {
            if (_fitOnAwake)
                Fit();
        }
    }
}