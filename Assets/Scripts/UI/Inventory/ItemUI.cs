﻿using System;
using Sirenix.OdinInspector;
using Unbowed.Gameplay.Characters.Items;
using Unbowed.Utility;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Unbowed.UI.Inventory {
    [RequireComponent(typeof(RectTransform))]
    public class ItemUI : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler {
        [SerializeField] float _cellSize = 47.5f;
        [SerializeField] float _cellSpacing = 2f;
        [SerializeField] Image _image;
        [SerializeField] RectTransform _raycastReceiver;
        [SerializeField, ChildGameObjectsOnly] DescriptionUI _descriptionUI;

        RectTransform _rectTransform;
        Canvas _canvas;

        public event Action<ItemUI, bool, PointerEventData> IsHoveredChanged;
        public event Action<ItemUI, PointerEventData> Dragged;

        public Item Item { get; private set; }

        public void SetItem(Item item) {
            Item = item;
            _descriptionUI.SetItem(Item);
            _image.color = Item == null ? Color.clear : Color.white;
            if (Item == null) return;
            var size = Item.config.size;
            _rectTransform = GetComponent<RectTransform>();
            _rectTransform.ForceUpdateRectTransforms();
            _rectTransform.sizeDelta = (Vector2) size * _cellSize + (size - Vector2.one) * _cellSpacing;
            _rectTransform.ForceUpdateRectTransforms();
            _image.sprite = Item.config.icon;
            SetRaycastReceiverSize(_rectTransform.sizeDelta);
        }

        public void SetRaycastReceiverSize(Vector2 size) {
            _raycastReceiver.sizeDelta = size;
        }

        public void OnPointerEnter(PointerEventData eventData) {
            if (Item != null && !ItemDragger.Instance.IsDragging) _descriptionUI.Open();
            IsHoveredChanged?.Invoke(this, true, eventData);
        }

        public void OnPointerDown(PointerEventData eventData) {
            if (eventData.button != PointerEventData.InputButton.Left) return;
            Dragged?.Invoke(this, eventData);
        }

        public void OnPointerExit(PointerEventData eventData) {
            _descriptionUI.Close();
            IsHoveredChanged?.Invoke(this, false, eventData);
        }

        #region Editor

        [Button]
        void SetDebugItem(ItemConfig config) {
            if (config == null) {
                Debug.LogError("Config was null");
                return;
            }

            var item = new Item(config, ItemLocation.None);
            SetItem(item);
        }

        #endregion
    }
}