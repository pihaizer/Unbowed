using System;
using TMPro;
using Unbowed.Gameplay;
using Unbowed.Gameplay.Characters.Items.Configs;
using Unbowed.Gameplay.Items;
using Unbowed.Gameplay.Signals;
using Unbowed.SO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Unbowed.UI.ItemNameplates
{
    [RequireComponent(typeof(Button))]
    public class DroppedItemNameplateUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;

        [Inject] private SignalBus _bus;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(InvokeItemClicked);
        }

        private void InvokeItemClicked() => _bus.Fire(new DroppedItemClickedSignal(Item));

        public DroppedItem Item
        {
            get => _item;
            set => SetItem(value);
        }

        private DroppedItem _item;

        private void SetItem(DroppedItem item)
        {
            _item = item;
            text.text = item.Item.Name;
            text.color = new Color(item.Item.Color.r, item.Item.Color.g, item.Item.Color.b, 1);
        }
    }
}