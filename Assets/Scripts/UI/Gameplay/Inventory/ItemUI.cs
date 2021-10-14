using System;
using Sirenix.OdinInspector;
using Unbowed.Gameplay.Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Unbowed.UI.Gameplay.Inventory {
    [RequireComponent(typeof(RectTransform))]
    public class ItemUI : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler {
        [SerializeField] float _cellSize = 47.5f;
        [SerializeField] float _cellSpacing = 2f;
        [SerializeField] Image _image;
        [SerializeField] RectTransform _raycastReceiver;
        [FormerlySerializedAs("_descriptionUI"),SerializeField, ChildGameObjectsOnly] ItemDescriptionUI itemDescriptionUI;

        RectTransform _rectTransform;
        Canvas _canvas;

        public event Action<ItemUI, bool, PointerEventData> IsHoveredChanged;
        public event Action<ItemUI, PointerEventData> Dragged;

        public Item Item { get; private set; }

        public void SetItem(Item item) {
            Item = item;
            itemDescriptionUI.SetItem(Item);
            _image.color = Item == null ? Color.clear : Color.white;
            
            if (Item == null) {
                _image.sprite = null;
                return;
            }
            
            var size = Item.Config.size;
            _rectTransform = GetComponent<RectTransform>();
            _rectTransform.sizeDelta = (Vector2) size * _cellSize + (size - Vector2.one) * _cellSpacing;
            _image.sprite = Item.Config.icon;
            SetRaycastReceiverSize(_rectTransform.sizeDelta);
        }

        public void SetRaycastReceiverSize(Vector2 size) {
            _raycastReceiver.sizeDelta = size;
        }

        public void OnPointerEnter(PointerEventData eventData) {
            Debug.Log("Enter");
            if (Item != null && !ItemDragger.Instance.IsDragging) itemDescriptionUI.Open();
            IsHoveredChanged?.Invoke(this, true, eventData);
        }

        public void OnPointerDown(PointerEventData eventData) {
            if (eventData.button != PointerEventData.InputButton.Left) return;
            Dragged?.Invoke(this, eventData);
        }

        public void OnPointerExit(PointerEventData eventData) {
            Debug.Log("Exit");
            itemDescriptionUI.Close();
            IsHoveredChanged?.Invoke(this, false, eventData);
        }
    }
}