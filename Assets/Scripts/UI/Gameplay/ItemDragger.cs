using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unbowed.Gameplay.Characters.Items;
using Unbowed.Gameplay.Items;
using Unbowed.SO;
using Unbowed.UI.Gameplay.Items;
using Unbowed.UI.Signals;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;
using Item = Unbowed.Gameplay.Characters.Items.Item;

namespace Unbowed.UI.Gameplay
{
    public class ItemDragger : MonoBehaviour
    {
        private ItemUI _draggedItemUI;
        private RectTransform _dragRect;

        public Item Item { get; private set; }
        public bool IsDragging => Item != null;

        [Inject] private ItemsDropper _itemsDropper;
        [Inject] private SignalBus _signalBus;

        private void Awake()
        {
            _signalBus.Subscribe<ItemUiClickedSignal>(OnItemClicked);
        }

        private void OnItemClicked(ItemUiClickedSignal signal)
        {
            if (IsDragging) return;
            DragItemUi(signal.ItemUI);
        }

        public void DragItemUi(ItemUI itemUI)
        {
            if (IsDragging) return;
            Item item = itemUI.Item;
            Destroy(itemUI.gameObject);
            DragItem(item);
        }

        public void DragItem(Item item)
        {
            if (IsDragging) StopDragging();

            Item = item;
            if (Item == null) return;

            _draggedItemUI = Instantiate(UIConfig.Instance.itemUI, transform);
            if (_draggedItemUI.TryGetComponent(out GraphicRaycaster raycaster)) Destroy(raycaster);
            if (_draggedItemUI.TryGetComponent(out Canvas canvas)) canvas.sortingOrder++;
            foreach (var graphic in _draggedItemUI.GetComponentsInChildren<Graphic>()) graphic.raycastTarget = false;
            _draggedItemUI.SetItem(Item);
            _dragRect = _draggedItemUI.GetComponent<RectTransform>();
            _dragRect.anchorMin = _dragRect.anchorMax = Vector2.zero;
            _dragRect.pivot = Vector2.one / 2;

            Unbowed.Gameplay.Characters.Modules.Inventory.RemoveItem(Item);

            StartCoroutine(DragItemCoroutine());
        }

        private IEnumerator DragItemCoroutine()
        {
            MouseContext.blockedByDraggedItem = true;
            var raycastResults = new List<RaycastResult>();
            var cell = new Vector2Int(int.MaxValue, int.MaxValue);
            var pointerEventData = new PointerEventData(EventSystem.current);
            BagsUI bagsUI = null;

            bool isFirstFrame = true;
            bool mouseAlreadyUp = false;

            while (IsDragging)
            {
                _dragRect.position = Input.mousePosition; // / scaler.transform.localScale.x;

                pointerEventData.position = Input.mousePosition;
                EventSystem.current.RaycastAll(pointerEventData, raycastResults);

                if (raycastResults.Any(raycastResult => raycastResult.gameObject.TryGetComponent(out bagsUI)))
                {
                    cell = HandleOnBags(bagsUI);
                }

                if (!isFirstFrame && Input.GetMouseButtonDown(0))
                {
                    TryPlaceItem(bagsUI, cell);
                    mouseAlreadyUp = Input.GetMouseButtonUp(0);
                }

                yield return null;

                isFirstFrame = false;
                if (bagsUI == null || Item == null) continue;
                bagsUI.ResetCellsState();
            }

            _draggedItemUI = null;
            if (!mouseAlreadyUp) yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
            MouseContext.blockedByDraggedItem = false;
        }

        private Vector2Int HandleOnBags(BagsUI bagsUI)
        {
            var bagsRect = bagsUI.GetComponent<RectTransform>();
            var bagsSize = bagsRect.sizeDelta;
            var bagsPivot = bagsRect.pivot;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                bagsRect, Input.mousePosition,
                null, out var positionInRect);

            positionInRect += new Vector2(bagsSize.x * bagsPivot.x, bagsSize.y * bagsPivot.y);
            positionInRect.y = bagsSize.y - positionInRect.y;
            positionInRect -= _dragRect.sizeDelta / 2;
            positionInRect.x *= bagsUI.Size.x / bagsSize.x;
            positionInRect.y *= bagsUI.Size.y / bagsSize.y;

            var cell = new Vector2Int(Mathf.RoundToInt(positionInRect.x), Mathf.RoundToInt(positionInRect.y));

            var location = ItemLocation.InBag(bagsUI.Inventory, cell);

            if (!bagsUI.Inventory.IsInGrid(new RectInt(cell, Item.Config.size))) return cell;

            if (bagsUI.Inventory.CanMoveItemToLocation(Item, location, out var removedItem))
            {
                if (removedItem != null)
                {
                    bagsUI.SetAreaState(removedItem.location.position, removedItem.Size, CellUI.State.Replace);
                }
                else
                {
                    bagsUI.SetAreaState(cell, Item.Config.size, CellUI.State.Positive);
                }
            }
            else
            {
                bagsUI.SetAreaState(cell, Item.Config.size, CellUI.State.Error);
            }

            return cell;
        }

        private bool TryPlaceItem(BagsUI bagsUI, Vector2Int cell)
        {
            if (bagsUI == null)
            {
                if (EventSystem.current.IsPointerOverGameObject()) return false;

                _itemsDropper.DropItem(Item, ActivePlayer.GetTransform());
                StopDragging();
                return true;
            }

            if (!bagsUI.Inventory.TryMoveItemToLocation(_draggedItemUI.Item,
                    ItemLocation.InBag(bagsUI.Inventory, cell), out var removedItem))
                return false;

            if (removedItem != null)
            {
                Item = removedItem;
                _draggedItemUI.SetItem(Item);
                return true;
            }

            StopDragging();
            return true;
        }

        private void StopDragging()
        {
            Item = null;
            Destroy(_draggedItemUI.gameObject);
            _draggedItemUI = null;
            _dragRect = null;
        }
    }
}