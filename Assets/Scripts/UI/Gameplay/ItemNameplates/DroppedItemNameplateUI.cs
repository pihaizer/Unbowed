using System;
using TMPro;
using Unbowed.Gameplay;
using Unbowed.Gameplay.Characters.Items;
using Unbowed.SO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Unbowed.UI.ItemNameplates {
    [RequireComponent(typeof(Button))]
    public class DroppedItemNameplateUI : MonoBehaviour {
        [SerializeField] TMP_Text text;

        void Awake() {
            GetComponent<Button>().onClick.AddListener(InvokeItemClicked);
        }

        void InvokeItemClicked() => ItemsContext.Instance.droppedItemClicked?.Invoke(Item);

        public DroppedItem Item {
            get => _item;
            set => SetItem(value);
        }

        DroppedItem _item;

        void SetItem(DroppedItem item) {
            _item = item;
            text.text = item.Item.Name;
        }
    }
}