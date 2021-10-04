using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unbowed.Gameplay.Characters.Items;
using UnityEngine;

namespace Unbowed.UI.ItemNameplates {
    public class DroppedItemsContainerUI : MonoBehaviour {
        [SerializeField, ChildGameObjectsOnly] DroppedItemNameplateUI reference;

        readonly Dictionary<DroppedItem, DroppedItemNameplateUI> _shownItems =
            new Dictionary<DroppedItem, DroppedItemNameplateUI>();

        void Awake() {
            reference.gameObject.SetActive(false);
        }

        void Update() {
            foreach (var item in _shownItems) {
                item.Value.transform.position = Camera.main.WorldToScreenPoint(item.Key.transform.position);
            }
        }

        public void RequestItem(DroppedItem item, bool value) {
            if (_shownItems.ContainsKey(item) && !value) {
                Destroy(_shownItems[item].gameObject);
                _shownItems.Remove(item);
            } else if (!_shownItems.ContainsKey(item) && value) {
                var nameplate = Instantiate(reference, transform);
                nameplate.gameObject.SetActive(true);
                nameplate.Item = item;
                _shownItems.Add(item, nameplate);
            }
        }
    }
}