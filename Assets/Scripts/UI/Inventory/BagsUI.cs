using Sirenix.OdinInspector;
using Unbowed.Gameplay.Characters.Items;
using UnityEngine;

namespace Unbowed.UI.Inventory {
    public class BagsUI : MonoBehaviour {
        [SerializeField] BagSlotUI[] slots;
        [SerializeField] BagSlotUI reference;

        public BagSlotUI[] Slots => slots;

        int Size => Application.isPlaying ? _inventory.Size : editorSize;

        Gameplay.Characters.Modules.Inventory _inventory;

        public void Init(InventoryUI parent) {
            reference.gameObject.SetActive(false);

            _inventory = parent.Inventory; 
            _inventory.Changed += UpdateBags;

            UpdateBagsSize();

            for (int i = 0; i < slots.Length; i++) {
                slots[i].Init(new ItemLocation(_inventory, i));
            }

            UpdateBags();
        }

        void UpdateBags() {
            foreach (var slot in slots) {
                slot.SetItem(null);
            }

            foreach (var item in _inventory.InBags) {
                slots[item.location.indexInBag].SetItem(item);
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

        #region EDITOR

        [SerializeField, HideInPlayMode] int editorSize;

        void OnValidate() {
            if (Application.isPlaying) return;
            if (slots.Length != Size) {
                UpdateBagsSize();
            }
        }

        #endregion
    }
}