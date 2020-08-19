using UnityEngine;
using UnityEngine.SceneManagement;

namespace Playrika.GameFoundation.UI
{
    [RequireComponent(typeof(Canvas))]
    public abstract class WindowBase : MonoBehaviour
    {
        public Canvas canvas { get; private set; }
        
        public virtual void Close()
        {
            WindowManager.Close(this);
        }

        protected virtual void Awake()
        {
            canvas = GetComponent<Canvas>();
            UpdateCanvasCamera();
            SceneManager.activeSceneChanged += OnSceneChanged;
        }

        protected virtual void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChanged;
        }

        private void OnSceneChanged(Scene from, Scene to)
        {
            UpdateCanvasCamera();
        }

        private void UpdateCanvasCamera()
        {
            canvas.worldCamera = Camera.main;
        }
    }
}