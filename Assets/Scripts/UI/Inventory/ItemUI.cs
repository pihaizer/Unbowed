using System;
using Sirenix.OdinInspector;
using Unbowed.Gameplay.Characters.Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Unbowed.UI.Inventory {
    [RequireComponent(typeof(RectTransform))]
    public class ItemUI : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler {
        [SerializeField] float _cellSize = 47.5f;
        [SerializeField] float _cellSpacing = 2f;
        [SerializeField] Image _image;

        public event Action<ItemUI, bool, PointerEventData> IsHoveredChanged;
        public event Action<ItemUI, PointerEventData> Clicked;
        
        public Item Item { get; private set; }

        public void SetItem(Item item) {
            if (item == null) {
                _image.color = Color.clear;
                return;
            }
            
            Item = item;
            var size = Item.config.size;
            GetComponent<RectTransform>().ForceUpdateRectTransforms();
            GetComponent<RectTransform>().sizeDelta = (Vector2)size * _cellSize + (size - Vector2.one) * _cellSpacing;
            GetComponent<RectTransform>().ForceUpdateRectTransforms();
            _image.sprite = Item.config.icon;
        }

        public void OnPointerEnter(PointerEventData eventData) {
            IsHoveredChanged?.Invoke(this, true, eventData);
        }

        public void OnPointerClick(PointerEventData eventData) {
            Clicked?.Invoke(this, eventData);
        }

        public void OnPointerExit(PointerEventData eventData) {
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