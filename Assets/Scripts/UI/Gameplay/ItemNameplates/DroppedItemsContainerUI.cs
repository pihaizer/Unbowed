using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unbowed.Gameplay.Items;
using Unbowed.Signals;
using Unbowed.SO;
using UnityEngine;
using Zenject;

namespace Unbowed.UI.ItemNameplates {
    public class DroppedItemsContainerUI : MonoBehaviour {
        [SerializeField] private DroppedItemNameplateUI nameplatePrefab;

        private readonly Dictionary<DroppedItem, DroppedItemNameplateUI> _shownItems = new();

        [Inject] private SignalBus _bus;

        private void Awake() {
            _bus.Subscribe<DescriptionCreateRequestSignal>(OnCreateItemRequest);
            _bus.Subscribe<DescriptionShowRequestSignal>(OnShowItemRequest);
        }

        private void OnDestroy() {
            _bus.Unsubscribe<DescriptionCreateRequestSignal>(OnCreateItemRequest);
            _bus.Unsubscribe<DescriptionShowRequestSignal>(OnShowItemRequest);
        }

        private void Update() {
            foreach (var item in _shownItems) {
                item.Value.transform.position = Camera.main.WorldToScreenPoint(item.Key.transform.position);
            }
        }

        private void OnShowItemRequest(DescriptionShowRequestSignal signal) {
            if (!_shownItems.ContainsKey(signal.Item)) {
                if (signal.IsShow)
                    OnCreateItemRequest(new DescriptionCreateRequestSignal(signal.Item, true));
                else
                    return;
            }

            _shownItems[signal.Item].gameObject.SetActive(signal.IsShow);
        }

        private void OnCreateItemRequest(DescriptionCreateRequestSignal signal) {
            if (_shownItems.ContainsKey(signal.Item) && !signal.IsCreate) {
                Destroy(_shownItems[signal.Item].gameObject);
                _shownItems.Remove(signal.Item);
            } else if (!_shownItems.ContainsKey(signal.Item) && signal.IsCreate) {
                var nameplate = Instantiate(nameplatePrefab, transform);
                nameplate.Item = signal.Item;
                nameplate.transform.position = Camera.main.WorldToScreenPoint(signal.Item.transform.position);
                _shownItems.Add(signal.Item, nameplate);
            }
        }
    }
}