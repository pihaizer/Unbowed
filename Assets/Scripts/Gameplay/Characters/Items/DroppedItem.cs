using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Unbowed.Gameplay.Characters.Modules;
using Unbowed.SO;
using Unbowed.Utility;
using UnityEngine;
using Random = System.Random;

namespace Unbowed.Gameplay.Characters.Items {
    public class DroppedItem : SerializedMonoBehaviour, IInteractable {
        [SerializeField, Range(0, 20f)] float throwForce = 1f;
        [SerializeField, MinMaxSlider(0, 20f)] Vector2 throwTorqueRange;
        [SerializeField, NonSerialized, OdinSerialize] Item startItem;

        public Item Item { get; private set; }

        GameObject _model;

        void Awake() {
            if (startItem != null) SetItem(startItem);
        }

        public void SetItem(Item item) {
            Item = item;
            _model = Instantiate(item.config.modelPrefab, transform);
            OnPickupError();
        }

        void Update() {
            if (Input.GetKey(KeyCode.LeftAlt)) {
                if (!RectUtils.One.Contains(Camera.main.WorldToViewportPoint(transform.position))) return;
                GlobalContext.Instance.descriptionRequest?.Invoke(this, true);
            }

            if (Input.GetKeyUp(KeyCode.LeftAlt)) OnMouseExit();
        }

        void OnDestroy() {
            OnMouseExit();
        }

        void OnMouseOver() {
            if (MouseState.Instance.BlockedByUI) return;
            GlobalContext.Instance.descriptionRequest?.Invoke(this, true);
        }

        void OnMouseExit() {
            GlobalContext.Instance.descriptionRequest?.Invoke(this, false);
        }


        public void Interact(GameObject source) {
            if (!source.TryGetComponent(out Inventory inventory)) return;
            if (inventory.TryAddItemToBags(Item)) {
                Destroy(gameObject);
            } else {
                OnPickupError();
            }
        }

        [Button]
        void OnPickupError() {
            if (!TryGetComponent(out Rigidbody rigidbody)) return;
            rigidbody.AddForce(Vector3.up * throwForce, ForceMode.Impulse);
            rigidbody.AddTorque(new Vector3(VectorRandom.Range(throwTorqueRange),
                VectorRandom.Range(throwTorqueRange),
                VectorRandom.Range(throwTorqueRange)));
        }

        public Transform GetTransform() => transform;
    }
}