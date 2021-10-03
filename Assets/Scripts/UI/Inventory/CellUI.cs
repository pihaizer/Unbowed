using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Sirenix.OdinInspector;
using Unbowed.Gameplay.Characters.Items;
using Unbowed.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Unbowed {
    public class CellUI : MonoBehaviour {
        [SerializeField, ChildGameObjectsOnly] Image _background;

        public Item Item { get; private set; }

        TweenerCore<Color, Color, ColorOptions> _tweener;
        State _state;

        public void Init() {
            SetItem(null);
        }

        void Update() => UpdateColor();

        public virtual void SetItem(Item item) {
            Item = item;
            if (Item == null) ResetState();
        }

        public void SetState(State state) {
            _state = state;
        }

        public void ResetState() => SetState(State.Default);

        void UpdateColor() {
            var color = Item?.Color ?? UIConfig.Instance.defaultSlotColor;

            color = _state switch {
                State.Default => color,
                State.Hover =>
                    Color.Lerp(color, UIConfig.Instance.hoverSlotColor, UIConfig.Instance.hoverBlend),
                State.Positive =>
                    Color.Lerp(color, UIConfig.Instance.positiveSlotColor, UIConfig.Instance.positiveBlend),
                State.Error =>
                    Color.Lerp(color, UIConfig.Instance.errorSlotColor, UIConfig.Instance.errorBlend),
                _ => throw new ArgumentOutOfRangeException()
            };

            _tweener.Kill();
            _tweener = _background.DOColor(color, UIConfig.Instance.itemsAnimationTime);
            // if (color == _background.color) return;
            // if (_tweener.IsActive()) {
            //     _tweener.endValue = color;
            //     _tweener.Restart();
            // } else 
        }

        public enum State {
            Default,
            Hover,
            Positive,
            Error,
        }
    }
}