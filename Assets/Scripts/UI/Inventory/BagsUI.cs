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

        int Size => Application.isPlaying ? _displayedInventory.inventoryItems.Length : editorSize;

        InventoryModule _displayedInventory;

        void OnEnable() {
            reference.gameObject.SetActive(false);

            if (!GlobalContext.Instance.playerCharacter ||
                !GlobalContext.Instance.playerCharacter.IsStarted) {
                return;
            }

            _displayedInventory = GlobalContext.Instance.playerCharacter.inventory;
            _displayedInventory.Changed += UpdateBags;

            UpdateBagsSize();

            for (int i = 0; i < slots.Length; i++) {
                slots[i].Init(i);
                slots[i].Clicked += OnSlotClicked;
            }

            UpdateBags();

            var testItem = new Item();
            testItem.config = testItemConfig;
            _displayedInventory.SetItem(0, testItem);
            _displayedInventory.SetItem(1, testItem);

            var testItem2 = new Item();
            testItem2.config = testItemConfig2;
            _displayedInventory.SetItem(3, testItem2);
            _displayedInventory.SetItem(4, testItem2);

            Debug.Log("Started bags UI");
        }

        void OnDisable() {
            if (_displayedInventory != null) _displayedInventory.Changed -= UpdateBags;
        }

        void OnSlotClicked(BagSlotUI slot, PointerEventData data) {
            if (data.button == PointerEventData.InputButton.Right) {
                if (!_displayedInventory.TryEquip(slot.Index)) {
                    slot.AnimateError();
                }
            }
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