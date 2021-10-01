using System;
using Sirenix.OdinInspector;
using Unbowed.Gameplay.Characters.Items;
using Unbowed.Gameplay.Characters.Modules;
using Unbowed.SO;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Unbowed {
    public class BagsUI : MonoBehaviour {
        [SerializeField] BagSlotUI[] slots;
        [SerializeField] BagSlotUI reference;

        [SerializeField, HideInPlayMode] int editorSize;
        [SerializeField] ItemConfig testItemConfig;
        [SerializeField] ItemConfig testItemConfig2;

        public BagSlotUI[] Slots => slots;

        int Size => Application.isPlaying ? _displayedInventory.inventoryItems.Length : editorSize;

        Inventory _displayedInventory;

        public void Init(InventoryUI parent) {
            reference.gameObject.SetActive(false);

            _displayedInventory = parent.Inventory; 
            _displayedInventory.Changed += UpdateBags;

            UpdateBagsSize();

            for (int i = 0; i < slots.Length; i++) {
                slots[i].Init(parent, i);
            }

            var testItem = new Item {config = testItemConfig};
            _displayedInventory.SetItem(0, testItem);
            _displayedInventory.SetItem(1, testItem);

            var testItem2 = new Item {config = testItemConfig2};
            _displayedInventory.SetItem(3, testItem2);
            _displayedInventory.SetItem(4, testItem2);

            UpdateBags();
        }

        void UpdateBags() {
            for (int i = 0; i < Size; i++) {
                slots[i].SetItem(_displayedInventory.inventoryItems[i]);
            }
        }

        void UpdateBagsSize() {
            foreach (var slot in slots) {
                if (Application.isPlaying)
                    Destroy(slot.gameObject);
                else
                    DestroyImmediate(slot.gameObject);
            }

            slots = new BagSlotUI[Size];

            for (int i = 0; i < Size; i++) {
                var slot = Instantiate(reference, transform);
                slot.gameObject.SetActive(true);
                slot.name = $"Slot_{i + 1}";
                slots[i] = slot;
            }
        }

        void OnValidate() {
            if (Application.isPlaying) return;
            if (slots.Length != Size) {
                UpdateBagsSize();
            }
        }
    }
}