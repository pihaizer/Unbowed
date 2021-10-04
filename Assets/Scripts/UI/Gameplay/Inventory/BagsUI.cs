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
    public class BagsUI : Menu {
        [OdinSerialize, ChildGameObjectsOnly] CellUI _cellReference;
        [OdinSerialize, AssetsOnly] ItemUI _itemUIPrefab;

        // ReSharper disable once Unity.RedundantHideInInspectorAttribute
        [OdinSerialize, HideInInspector] CellUI[,] _cells;

        [ShowInInspector]
        public List<ItemUI> ItemUIs { get; private set; }

        public Gameplay.Characters.Modules.Inventory Inventory { get; private set; }

        public Vector2Int Size { get; private set; }

        public void SetInventory(Gameplay.Characters.Modules.Inventory inventory) {
            if (Inventory != null) {
                Inventory.AddedItem -= AddItem;
                Inventory.RemovedItem -= RemoveItem;
                
                foreach (var itemUI in ItemUIs) {
                    Destroy(itemUI.gameObject);
                }
            }
            
            Inventory = inventory;
            
            if (Inventory == null) return;

            _cellReference.gameObject.SetActive(false);
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

            _cells = new CellUI[Size.x, Size.y];

            for (int j = 0; j < Size.y; j++) {
                for (int i = 0; i < Size.x; i++) {
                    var cell = _cells[i, j] = Instantiate(_cellReference);
                    cell.Init();
                    cell.gameObject.SetActive(true);
                    cell.transform.SetParent(transform, false);
                }
            }

            var rectTransform = GetComponent<RectTransform>();
            var grid = GetComponent<GridLayoutGroup>();

            float width = grid.cellSize.x * Size.x + grid.spacing.x * (Size.x - 1) + grid.padding.horizontal;
            float height = grid.cellSize.y * Size.y + grid.spacing.y * (Size.y - 1) + grid.padding.vertical;
            rectTransform.sizeDelta = new Vector2(width, height);
        }

        void SetItems(IEnumerable<Item> items) {
            ItemUIs = new List<ItemUI>();
            foreach (var item in items) {
                if (!item.IsInBags) continue;
                AddItem(item);
            }
        }

        void AddItem(Item item) {
            if (item.IsEquipped) return;

            var itemUI = Instantiate(_itemUIPrefab, transform);
            itemUI.SetItem(item);
            itemUI.IsHoveredChanged += OnItemHoveredChanged;
            itemUI.Dragged += OnItemDragged;
            ItemUIs.Add(itemUI);

            foreach (var position in new RectInt(item.location.position, item.config.size).allPositionsWithin) {
                _cells[position.x, position.y].SetItem(item);
            }

            itemUI.transform.SetParent(_cells[item.location.position.x, item.location.position.y].transform, false);
        }

        void RemoveItem(Item item) {
            if (item.IsEquipped) return;

            var uiToRemove = ItemUIs.Find(ui => ui.Item == item);
            // if (!uiToRemove) return;
            ItemUIs.Remove(uiToRemove);
            Destroy(uiToRemove.gameObject);
            var rect = new RectInt(item.location.position, item.config.size);
            foreach (var cell in rect.allPositionsWithin) {
                _cells[cell.x, cell.y].SetItem(null);
            }
        }

        void OnItemHoveredChanged(ItemUI itemUI, bool value, PointerEventData data) {
            if (ItemDragger.Instance.IsDragging) return;
            SetAreaState(itemUI.Item.location.position, itemUI.Item.config.size,
                value ? CellUI.State.Hover : CellUI.State.Default);
        }

        public void SetAreaState(Vector2Int position, Vector2Int size, CellUI.State state) =>
            SetAreaAction(position, size, ui => ui.SetState(state));

        public void ResetCellsState() {
            foreach (var cellUI in _cells) cellUI.ResetState();
        }

        public void ResetAreaState(Vector2Int position, Vector2Int size) =>
            SetAreaAction(position, size, ui => ui.ResetState());

        void SetAreaAction(Vector2Int position, Vector2Int size, Action<CellUI> action) {
            var rect = new RectInt(position, size);
            foreach (var cell in rect.allPositionsWithin) {
                if (cell.x < 0 || cell.y < 0) return;
                if (cell.x >= Size.x || cell.y >= Size.y) return;
                action(_cells[cell.x, cell.y]);
            }
        }

        void OnItemDragged(ItemUI itemUI, PointerEventData pointerEventData) {
            if (ItemDragger.Instance.IsDragging) return;
            ItemDragger.Instance.DragItem(itemUI.Item);
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