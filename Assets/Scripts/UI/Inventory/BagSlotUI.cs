using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unbowed.Gameplay.Characters.Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Unbowed {
    public class BagSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, 
        IDragHandler, IBeginDragHandler, IEndDragHandler {
        [SerializeField] Image background;
        [SerializeField] Image icon;
        [SerializeField] Color hoverColor;
        [SerializeField] float hoverAnimationTime;
        [SerializeField] Color errorColor;
        [SerializeField] float errorAnimationTime;
        [SerializeField] float minDragDelta;

        public event Action<BagSlotUI, PointerEventData> Entered;
        public event Action<BagSlotUI, PointerEventData> Exited;
        public event Action<BagSlotUI, PointerEventData> Clicked;
        public event Action<BagSlotUI, PointerEventData> Dragged;

        public InventoryUI Parent { get; private set; }
        public int Index { get; private set; }
        public EquipmentSlot EquipmentSlot { get; private set; }
        public SlotLocation Location { get; private set; }
        public Item Item { get; private set; }

        Color _defaultColor;
        bool _isHovered;
        bool _isDragged;
        Canvas _tempCanvas;
        

        public void Init(InventoryUI parent, int index) {
            Init(parent);
            Index = index;
            Location = SlotLocation.Bags;
        }
        
        public void Init(InventoryUI parent, EquipmentSlot equipmentSlot) {
            Init(parent);
            EquipmentSlot = equipmentSlot;
            Location = SlotLocation.Equipment;
        }

        void Init(InventoryUI parent) {
            Parent = parent;
            SetItem(null);
            _defaultColor = background.color;
        }

        public void SetItem(Item item) {
            Item = item;
            icon.gameObject.SetActive(Item != null);
            if (Item != null) {
                icon.sprite = Item.config.icon;
            }
        }
        
        public void OnPointerEnter(PointerEventData eventData) {
            Entered?.Invoke(this, eventData);
            _isHovered = true;
            background.DOColor(hoverColor, hoverAnimationTime);
        }

        public void OnPointerClick(PointerEventData eventData) {
            Clicked?.Invoke(this, eventData);
        }

        public void OnPointerExit(PointerEventData eventData) {
            Exited?.Invoke(this, eventData);
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

        public void OnBeginDrag(PointerEventData eventData) {
            _isDragged = true;
            _tempCanvas = gameObject.AddComponent<Canvas>();
            _tempCanvas.overrideSorting = true;
            _tempCanvas.sortingOrder = 10;
        }

        public void OnDrag(PointerEventData eventData) {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(background.rectTransform,
                eventData.position, null, out var worldPoint);
            icon.transform.position = worldPoint;
        }

        public void OnEndDrag(PointerEventData eventData) {
            _isDragged = false;
            Destroy(_tempCanvas);
            icon.transform.localPosition = Vector3.zero;
            Dragged?.Invoke(this, eventData);
        }

        public enum SlotLocation {
            Bags,
            Equipment
        }
    }
}