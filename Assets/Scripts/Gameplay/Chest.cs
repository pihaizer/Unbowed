using System;
using System.Collections.Generic;
using Unbowed.Gameplay.Characters.Modules;
using Unbowed.SO;
using UnityEngine;

namespace Unbowed.Gameplay {
    [RequireComponent(typeof(Inventory))]
    public class Chest : MonoBehaviour, ISelectable, IInteractable {
        [SerializeField] private float maxDistance = 3f;

        private Inventory _inventory;
        private bool _isOpened = true;
        private GameObject _opener;

        private void Start() {
            _inventory = GetComponent<Inventory>();
            _inventory.Init();
        }

        private void Update() {
            if (!_isOpened) return;
            
            if (_opener == null) {
                _isOpened = false;
                return;
            }

            if (!(Vector3.Distance(transform.position, _opener.transform.position) > maxDistance)) 
                return;
            
            EventsContext.Instance.otherInventoryRequest?.Invoke(_inventory, false);
            _isOpened = false;
            _opener = null;
        }

        public void Interact(GameObject source) {
            EventsContext.Instance.otherInventoryRequest?.Invoke(_inventory, true);
            _isOpened = true;
            _opener = source;
        }

        public Transform GetTransform() => transform;

        public string GetName() => name;

        public bool CanBeSelected() => true;

        public bool HasTargetUI() => true;
    }
}