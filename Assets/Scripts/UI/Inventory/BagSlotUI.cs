using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unbowed.Gameplay.Characters.Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Unbowed {
    public class BagSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
        [SerializeField] Image background;
        [SerializeField] Image icon;
        [SerializeField] Color hoverColor;
        [SerializeField] float hoverAnimationTime;
        [SerializeField] Color errorColor;
        [SerializeField] float errorAnimationTime;

        public event Action<BagSlotUI, PointerEventData> Clicked;

        public int Index { get; private set; }

        Color _defaultColor;
        bool _isHovered;
        

        public void Init(int index) {
            Index = index;
            SetItem(null);
            _defaultColor = background.color;
        }

        public void SetItem(Item item) {
            icon.gameObject.SetActive(item != null);
            if (item != null) {
                icon.sprite = item.config.icon;
            }
        }

        public void OnPointerEnter(PointerEventData eventData) {
            _isHovered = true;
            background.DOColor(hoverColor, hoverAnimationTime);
        }

        public void OnPointerClick(PointerEventData eventData) {
            Clicked?.Invoke(this, eventData);
        }

        public void OnPointerExit(PointerEventData eventData) {
            _isHovered = false;
            background.DOComplete(true);
            background.DOColor(_defaultColor, hoverAnimationTime);
        }

        public void AnimateError() {
            background.DOComplete();
            var sequence = DOTween.Sequence(background);
            sequence.Append(background.DOColor(errorColor, errorAnimationTime / 2));
            sequence.Append(background.DOColor(_isHovered ? hoverColor : _defaultColor, errorAnimationTime / 2));
        }
    }
}