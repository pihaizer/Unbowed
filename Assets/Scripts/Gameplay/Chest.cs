using System;
using System.Collections.Generic;
using Unbowed.Gameplay.Characters.Modules;
using Unbowed.Signals;
using Unbowed.SO;
using Unbowed.UI.Signals;
using Unbowed.Utility;
using UnityEngine;
using Zenject;

namespace Unbowed.Gameplay {
    [RequireComponent(typeof(Inventory))]
    public class Chest : MonoBehaviour, ISelectable, IInteractable {
        [SerializeField] private float maxDistance = 3f;

        [Inject] private SignalBus _bus;

        private Inventory _inventory;
        private GameObject _opener;

        public readonly Mutable<bool> IsOpened = new();

        private void Start() {
            _inventory = GetComponent<Inventory>();
            _inventory.Init();
        }

        private void Update() {
            if (!IsOpened) return;
            
            if (_opener == null) {
                IsOpened.Value = false;
                return;
            }

            if (!(Vector3.Distance(transform.position, _opener.transform.position) > maxDistance)) 
                return;
            
            _bus.Fire(new ScreenActionSignal(ScreenNames.OtherInventory, ScreenAction.Close, _inventory));
            IsOpened.Value = false;
            _opener = null;
        }

        public void Interact(GameObject source) {
            _bus.Fire(new ScreenActionSignal(ScreenNames.OtherInventory, ScreenAction.Open, _inventory));
            IsOpened.Value = true;
            _opener = source;
        }

        public Transform GetTransform() => transform;

        public string GetName() => name;

        public bool CanBeSelected() => true;

        public bool HasTargetUI() => true;
    }
}