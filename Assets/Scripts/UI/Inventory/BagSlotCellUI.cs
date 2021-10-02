using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unbowed.Gameplay.Characters.Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Unbowed {
    public class BagSlotCellUI : MonoBehaviour {
        [SerializeField] Image _background;
        [SerializeField] Color _hoverColor;
        [SerializeField] Color _itemColor;
        [SerializeField] float _animationTime;

        public Item Item { get; private set; }

        Color _defaultColor;
        bool _isHovered;

        public void Init() {
            _defaultColor = _background.color;
            SetItem(null);
        }

        public void SetItem(Item item) {
            Item = item;
            _isHovered = false;
            UpdateColor();
        }

        public void SetHovered(bool value) {
            _isHovered = value;
            UpdateColor();
        }

        void UpdateColor() {
            _background.DOKill();
            Color color;
            if (_isHovered)
                color = _hoverColor;
            else if (Item != null)
                color = _itemColor;
            else
                color = _defaultColor;
            _background.DOColor(color, _animationTime);
        }
    }
}