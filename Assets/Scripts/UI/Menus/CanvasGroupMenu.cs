using UnityEngine;

namespace Unbowed.UI {
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasGroupMenu : Menu {
        private CanvasGroup _canvasGroup;
        
        protected override void Awake() {
            _canvasGroup = GetComponent<CanvasGroup>();
            base.Awake();
        }

        protected override void SetOpened(bool value) {
            if (!isStarted) Awake();
            IsOpened.Set(value);
            _canvasGroup.alpha = IsOpened ? 1 : 0;
            _canvasGroup.blocksRaycasts = IsOpened;
        }
    }
}