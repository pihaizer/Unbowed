using System;
using Sirenix.OdinInspector;
using Unbowed.Utility;
using UnityEngine;

namespace Unbowed.UI {
    public class Menu : SerializedMonoBehaviour {
        [SerializeField] bool startOpened;

        public Mutable<bool> IsOpened { get; } = new Mutable<bool>();

        protected bool isStarted;

        protected virtual void Awake() {
            if (isStarted) return;
            isStarted = true;
            SetOpened(startOpened);
        }

        public void Open() => SetOpened(true);

        public void Close() => SetOpened(false);

        [Button]
        public void ToggleOpened() => SetOpened(!IsOpened);

        protected virtual void SetOpened(bool value) {
            if (!isStarted) Awake();
            gameObject.SetActive(value);
            IsOpened.Set(value);
        }
    }
}