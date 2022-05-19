using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Sirenix.OdinInspector;
using Unbowed.Gameplay.Items;
using UnityEngine;
using UnityEngine.UI;

using Item = Unbowed.Gameplay.Characters.Items.Item;

namespace Unbowed.UI.Gameplay.Inventory {
    public class CellUI : MonoBehaviour {
        [SerializeField, ChildGameObjectsOnly] private Image _background;

        public Item Item { get; private set; }

        private TweenerCore<Color, Color, ColorOptions> _tweener;
        private State _state;

        public void Init() {
            SetItem(null);
            _background.color = GetColor();
        }

        private void OnDestroy() {
            _tweener.Kill();
        }

        private void Update() => UpdateColor();

        public virtual void SetItem(Item item) {
            Item = item;
            if (Item == null) ResetState();
        }

        public void SetState(State state) {
            _state = state;
        }

        public void ResetState() => SetState(State.Default);

        private void UpdateColor() {
            var color = GetColor();
            _background.color = color;
            // _tweener.Kill();
            // _tweener = _background.DOColor(color, UIConfig.Instance.itemsAnimationTime);
        }

        private Color GetColor() {
            var color = Item?.Color ?? GetDefaultSlotColor();

            color = _state switch {
                State.Default => color,
                State.Hover =>
                    Color.Lerp(color, UIConfig.Instance.hoverSlotColor, UIConfig.Instance.hoverBlend),
                State.Positive =>
                    Color.Lerp(color, UIConfig.Instance.positiveSlotColor, UIConfig.Instance.positiveBlend),
                State.Replace =>
                    Color.Lerp(color, UIConfig.Instance.replaceSlotColor, UIConfig.Instance.replaceBlend),
                State.Error =>
                    Color.Lerp(color, UIConfig.Instance.errorSlotColor, UIConfig.Instance.errorBlend),
                _ => throw new ArgumentOutOfRangeException()
            };

            return color;
        }

        protected virtual Color GetDefaultSlotColor() => UIConfig.Instance.defaultBagsSlotColor; 

        public enum State {
            Default,
            Hover,
            Positive,
            Replace,
            Error,
        }
    }
}