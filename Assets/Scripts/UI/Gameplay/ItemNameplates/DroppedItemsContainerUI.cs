using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unbowed.Gameplay.Characters.Items;
using Unbowed.SO;
using UnityEngine;

namespace Unbowed.UI.ItemNameplates {
    public class DroppedItemsContainerUI : MonoBehaviour {
        [SerializeField, ChildGameObjectsOnly] DroppedItemNameplateUI reference;

        readonly Dictionary<DroppedItem, DroppedItemNameplateUI> _shownItems =
            new Dictionary<DroppedItem, DroppedItemNameplateUI>();

        void Awake() {
            reference.gameObject.SetActive(false);
            EventsContext.Instance.descriptionCreateRequest += RequestCreateItem;
            EventsContext.Instance.descriptionShowRequest += RequestShowItem;
        }

        void OnDestroy() {
            EventsContext.Instance.descriptionCreateRequest -= RequestCreateItem;
            EventsContext.Instance.descriptionShowRequest -= RequestShowItem;
        }

        void Update() {
            foreach (var item in _shownItems) {
                item.Value.transform.position = Camera.main.WorldToScreenPoint(item.Key.transform.position);
            }
        }

        void RequestShowItem(DroppedItem item, bool value) {
            if (!_shownItems.ContainsKey(item)) {
                if (value)
                    RequestCreateItem(item, true);
                else
                    return;
            }

            _shownItems[item].gameObject.SetActive(value);
        }

        void RequestCreateItem(DroppedItem item, bool value) {
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