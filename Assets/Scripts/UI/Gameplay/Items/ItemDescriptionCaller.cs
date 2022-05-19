using System;
using Sirenix.OdinInspector;
using Unbowed.Gameplay.Characters.Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Unbowed.UI.Gameplay.Items
{
    [RequireComponent(typeof(Graphic))]
    public class ItemDescriptionCaller : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField, AssetsOnly]
        private ItemDescriptionUI itemDescriptionPrefab;

        [Inject] private PopupFactory _popupFactory;
        [Inject] private ItemDragger _itemDragger;

        public Item Item { get; private set; }

        private ItemDescriptionUI _itemDescriptionUI;
        private bool _isHovered;

        protected void OnDestroy()
        {
            ClearItemDescription();
        }

        public void SetItem(Item item)
        {
            Item = item;
            if (Item == null) ClearItemDescription();
        }

        private void Update()
        {
            if (_isHovered && _itemDescriptionUI == null) ShowItemDescription();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _isHovered = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isHovered = false;
            ClearItemDescription();
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
    }
}