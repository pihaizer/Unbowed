using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unbowed.Gameplay.Items;
using Unbowed.SO;
using UnityEngine;

namespace Unbowed.UI.ItemNameplates {
    public class DroppedItemsContainerUI : MonoBehaviour {
        [SerializeField, ChildGameObjectsOnly] private DroppedItemNameplateUI reference;

        private readonly Dictionary<DroppedItem, DroppedItemNameplateUI> _shownItems =
            new Dictionary<DroppedItem, DroppedItemNameplateUI>();

        private void Awake() {
            reference.gameObject.SetActive(false);
            EventsContext.Instance.descriptionCreateRequest += RequestCreateItem;
            EventsContext.Instance.descriptionShowRequest += RequestShowItem;
        }

        private void OnDestroy() {
            EventsContext.Instance.descriptionCreateRequest -= RequestCreateItem;
            EventsContext.Instance.descriptionShowRequest -= RequestShowItem;
        }

        private void Update() {
            foreach (var item in _shownItems) {
                item.Value.transform.position = Camera.main.WorldToScreenPoint(item.Key.transform.position);
            }
        }

        private void RequestShowItem(DroppedItem item, bool value) {
            if (!_shownItems.ContainsKey(item)) {
                if (value)
                    RequestCreateItem(item, true);
                else
                    return;
            }

            _shownItems[item].gameObject.SetActive(value);
        }

        private void RequestCreateItem(DroppedItem item, bool value) {
            if (_shownItems.ContainsKey(item) && !value) {
                Destroy(_shownItems[item].gameObject);
                _shownItems.Remove(item);
            } else if (!_shownItems.ContainsKey(item) && value) {
                var nameplate = Instantiate(reference, transform);
                nameplate.Item = item;
                nameplate.transform.position = Camera.main.WorldToScreenPoint(item.transform.position);
                _shownItems.Add(item, nameplate);
            }
        }
    }
}