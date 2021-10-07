using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Unbowed.Gameplay.Characters.Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Unbowed.UI.Gameplay.Inventory {
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(GridLayoutGroup))]
    public class BagsUI : Menu {
        [OdinSerialize, ChildGameObjectsOnly] CellUI _cellReference;
        [OdinSerialize, AssetsOnly] ItemUI _itemUIPrefab;

        // ReSharper disable once Unity.RedundantHideInInspectorAttribute
        [OdinSerialize, HideInInspector] CellUI[] _cells;

        [ShowInInspector]
        public List<ItemUI> ItemUIs { get; private set; }

        public Unbowed.Gameplay.Characters.Modules.Inventory Inventory { get; private set; }

        public Vector2Int Size { get; private set; }

        public void SetInventory(Unbowed.Gameplay.Characters.Modules.Inventory inventory) {
            if (Inventory == inventory) return;
            
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
            int oldSize1d = Size.x * Size.y;
            Size = size;
            int newSize1d = Size.x * Size.y;

            var rectTransform = GetComponent<RectTransform>();
            var grid = GetComponent<GridLayoutGroup>();

            float width = grid.cellSize.x * Size.x + grid.spacing.x * (Size.x - 1) + grid.padding.horizontal;
            float height = grid.cellSize.y * Size.y + grid.spacing.y * (Size.y - 1) + grid.padding.vertical;
            rectTransform.sizeDelta = new Vector2(width, height);

            if (oldSize1d == newSize1d) return;

            for (int i = newSize1d; i < oldSize1d; i++) {
                if (_cells[i] == null) continue;
                if (Application.isEditor)
                    DestroyImmediate(_cells[i].gameObject);
                else
                    Destroy(_cells[i].gameObject);
            }

            var oldCells = _cells;
            _cells = new CellUI[newSize1d];

            for (int i = 0; i < Math.Min(oldSize1d, newSize1d); i++) {
                _cells[i] = oldCells[i];
            }

            for (int i = oldSize1d; i < newSize1d; i++) {
                var cell = Instantiate(_cellReference, transform, false);
                _cells[i] = cell;
                cell.Init();
                cell.gameObject.SetActive(true);
            }
        }

        CellUI GetCell(int x, int y) => _cells[y * Size.x + x];

        CellUI GetCell(Vector2Int position) => GetCell(position.x, position.y);

        void SetCell(int x, int y, CellUI cellUI) => _cells[y * Size.x + x] = cellUI;

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
                GetCell(position).SetItem(item);
            }

            itemUI.transform.SetParent(GetCell(item.location.position).transform, false);
        }

        void RemoveItem(Item item) {
            if (item.IsEquipped) return;

            var uiToRemove = ItemUIs.Find(ui => ui.Item == item);
            ItemUIs.Remove(uiToRemove);
            Destroy(uiToRemove.gameObject);
            var rect = new RectInt(item.location.position, item.config.size);
            foreach (var position in rect.allPositionsWithin) {
                GetCell(position).SetItem(null);
            }
        }

        void OnItemHoveredChanged(ItemUI itemUI, bool value, PointerEventData data) {
            if (ItemDragger.Instance.IsDragging) return;
            SetAreaState(itemUI.Item.location.position, itemUI.Item.config.size,
                value ? CellUI.State.Hover : CellUI.State.Default);
        }

        public void SetAreaState(Vector2Int min, Vector2Int size, CellUI.State state) =>
            SetAreaAction(min, size, ui => ui.SetState(state));

        public void ResetCellsState() {
            foreach (var cellUI in _cells) cellUI.ResetState();
        }

        public void ResetAreaState(Vector2Int min, Vector2Int size) =>
            SetAreaAction(min, size, ui => ui.ResetState());

        void SetAreaAction(Vector2Int min, Vector2Int size, Action<CellUI> action) {
            var rect = new RectInt(min, size);
            foreach (var position in rect.allPositionsWithin) {
                if (position.x < 0 || position.y < 0) return;
                if (position.x >= Size.x || position.y >= Size.y) return;
                action(GetCell(position));
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