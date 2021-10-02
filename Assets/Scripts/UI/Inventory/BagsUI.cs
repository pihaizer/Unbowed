using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Unbowed.Gameplay.Characters.Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Unbowed.UI.Inventory {
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(GridLayoutGroup))]
    public class BagsUI : SerializedMonoBehaviour {
        [OdinSerialize, ChildGameObjectsOnly] BagSlotCellUI _cellReference;
        [OdinSerialize, AssetsOnly] ItemUI _itemUIPrefab;
        
        // ReSharper disable once Unity.RedundantHideInInspectorAttribute
        [OdinSerialize, HideInInspector] BagSlotCellUI[,] _cells;
        
        public event Action<ItemUI, PointerEventData> ItemClicked;

        public List<ItemUI> ItemUIs { get; private set; }
        
        public Gameplay.Characters.Modules.Inventory Inventory { get; private set; }

        public Vector2Int Size { get; private set; }

        public void SetInventory(Gameplay.Characters.Modules.Inventory inventory) {
            _cellReference.gameObject.SetActive(false);
            Inventory = inventory;
            SetSize(Inventory.Size);
            SetItems(Inventory.Items);
            Inventory.AddedItem += AddItem;
            Inventory.RemovedItem += RemoveItem;
        }

        void SetSize(Vector2Int size) {
            Size = size;
            
            foreach (var cell in _cells) {
                if (cell != null) DestroyImmediate(cell.gameObject);
            }

            _cells = new BagSlotCellUI[Size.x, Size.y];

            for (int j = 0; j < Size.y; j++) {
                for (int i = 0; i < Size.x; i++) {
                    var cell = _cells[i, j] = Instantiate(_cellReference);
                    cell.Init();
                    cell.gameObject.SetActive(true);
                    cell.transform.SetParent(transform, false);
                }
            }
            
            var rectTransform =  GetComponent<RectTransform>();
            var grid =  GetComponent<GridLayoutGroup>();

            float height = grid.cellSize.y * Size.y + grid.spacing.y * (Size.y - 1) + grid.padding.vertical;
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, height);
        }

        void SetItems(IEnumerable<Item> items) {
            ItemUIs = new List<ItemUI>();
            foreach (var item in items) {
                if (!item.IsInBags) continue;
                AddItem(item);
            }
        }

        void AddItem(Item item) {
            var itemUI = Instantiate(_itemUIPrefab, transform);
            itemUI.SetItem(item);
            foreach (var position in new RectInt(item.location.position, item.config.size).allPositionsWithin) {
                _cells[position.x, position.y].SetItem(item);
            }
            itemUI.transform.SetParent(_cells[item.location.position.x, item.location.position.y].transform, false);
            ItemUIs.Add(itemUI);
            itemUI.IsHoveredChanged += OnItemHoveredChanged;
            itemUI.Clicked += OnItemClicked;
        }

        void OnItemHoveredChanged(ItemUI itemUI, bool value, PointerEventData data) {
            var rect = new RectInt(itemUI.Item.location.position, itemUI.Item.config.size);
            foreach (var cell in rect.allPositionsWithin) {
                _cells[cell.x, cell.y].SetHovered(value);
            }
        }

        public void SetAreaHovered(Vector2Int position, Vector2Int size, bool value) {
            var rect = new RectInt(position, size);
            foreach (var cell in rect.allPositionsWithin) {
                if (cell.x < 0 || cell.y < 0) return;
                if (cell.x >= Size.x || cell.y >= Size.y) return;
                _cells[cell.x, cell.y].SetHovered(value);
            }
        }

        void OnItemClicked(ItemUI itemUI, PointerEventData pointerEventData) {
            ItemClicked?.Invoke(itemUI, pointerEventData);
        }

        void RemoveItem(Item item) {
            var uiToRemove = ItemUIs.Find(ui => ui.Item == item);
            ItemUIs.Remove(uiToRemove);
            Destroy(uiToRemove.gameObject);
            var rect = new RectInt(item.location.position, item.config.size);
            foreach (var cell in rect.allPositionsWithin) {
                _cells[cell.x, cell.y].SetItem(null);
            }
        }
        
        #region Editor

        [NonSerialized, ShowInInspector, HideInPlayMode, OnValueChanged(nameof(UpdateSizeInEditor))]
        Vector2Int _editorSize;

        void UpdateSizeInEditor() {
            if (Application.isPlaying) throw new Exception("Invoked in game");
            SetSize(_editorSize);
        }

        #endregion
    }
}