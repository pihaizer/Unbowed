using System;
using Sirenix.OdinInspector;
using Unbowed.Utility;
using UnityEngine;

namespace Unbowed.UI {
    [RequireComponent(typeof(CanvasGroup))]
    public class Menu : SerializedMonoBehaviour {
        [SerializeField] bool startOpened;

        CanvasGroup _canvasGroup;

        public Mutable<bool> IsOpened { get; } = new Mutable<bool>();

        bool _isStarted;

        protected virtual void Start() {
            if (_isStarted) return;
            _canvasGroup = GetComponent<CanvasGroup>();
            _isStarted = true;
            SetOpened(startOpened);
        }

        public void Open() => SetOpened(true);

        public void Close() => SetOpened(false);

        [Button]
        public void ToggleOpened() => SetOpened(!IsOpened);

        public virtual void SetOpened(bool value) {
            if (!_isStarted) Start();
            IsOpened.Set(value);
            _canvasGroup.alpha = IsOpened ? 1 : 0;
            _canvasGroup.blocksRaycasts = IsOpened;
        }
    }
}