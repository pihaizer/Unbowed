using System;
using Sirenix.OdinInspector;
using Unbowed.SO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;
using Item = Unbowed.Gameplay.Characters.Items.Item;
using Object = UnityEngine.Object;

namespace Unbowed.UI.Gameplay.Items
{
    [RequireComponent(typeof(RectTransform))]
    public class ItemUI : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
    {
        [SerializeField] private float _cellSize = 47.5f;
        [SerializeField] private float _cellSpacing = 2f;
        [SerializeField] private Image _image;
        [SerializeField] private RectTransform _raycastReceiver;
        [SerializeField, AssetsOnly]
        private ItemDescriptionUI itemDescriptionPrefab;

        [Inject] private PopupFactory _popupFactory;
        [Inject] private ItemDragger _itemDragger;

        private ItemDescriptionUI _itemDescriptionUI;
        private RectTransform _rectTransform;

        public event Action<bool> IsHoveredChanged;
        public event Action Clicked;

        public Item Item { get; private set; }

        private void OnDestroy() => ClearItemDescription();

        public void SetItem(Item item)
        {
            Item = item;
            _image.color = Item == null ? Color.clear : Color.white;

            if (Item == null)
            {
                _image.sprite = null;
                return;
            }

            Vector2Int size = Item.Config.size;
            _rectTransform = GetComponent<RectTransform>();
            _rectTransform.sizeDelta = (Vector2) size * _cellSize + (size - Vector2.one) * _cellSpacing;
            _image.sprite = Item.Config.icon;
            SetRaycastReceiverSize(_rectTransform.sizeDelta);
        }

        public void SetRaycastReceiverSize(Vector2 size)
        {
            _raycastReceiver.sizeDelta = size;
        }

        public void OnPointerEnter(PointerEventData eventData) => SetHovered(true);

        public void OnPointerExit(PointerEventData eventData) => SetHovered(false);

        public void OnPointerDown(PointerEventData eventData)
        {
            Clicked?.Invoke();
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Right:
                    ActivePlayer.Get().TryUseItem(Item);
                    break;
                case PointerEventData.InputButton.Left:
                    _itemDragger.DragItemUi(this);
                    break;
            }
        }

        private void SetHovered(bool value)
        {
            if (value) ShowItemDescription();
            else ClearItemDescription();

            IsHoveredChanged?.Invoke(value);
        }

        private void ShowItemDescription()
        {
            if (Item == null || _itemDragger.IsDragging || _itemDescriptionUI != null) return;

            _itemDescriptionUI = _popupFactory.Create(itemDescriptionPrefab);
            _itemDescriptionUI.SetItem(Item);
            _itemDescriptionUI.AlignNear(GetComponent<RectTransform>());
        }

        private void ClearItemDescription()
        {
            if (_itemDescriptionUI != null) Destroy(_itemDescriptionUI.gameObject);
            _itemDescriptionUI = null;
        }
        
        public class Factory : PlaceholderFactory<Object, ItemUI> {}
    }
}