using Sirenix.OdinInspector;
using Unbowed.Gameplay.Characters.Modules;
using Unbowed.SO;
using Unbowed.Utility;
using UnityEngine;

namespace Unbowed.Gameplay.Items {
    public class DroppedItem : SerializedMonoBehaviour, IInteractable {
        [SerializeField, Range(0, 20f)] float throwForce = 1f;
        [SerializeField, MinMaxSlider(0, 20f)] Vector2 throwTorqueRange;

        public Characters.Items.Item Item { get; private set; }

        void Update() {
            if (Input.GetKey(KeyCode.LeftAlt)) {
                if (!RectUtils.One.Contains(Camera.main.WorldToViewportPoint(transform.position))) return;
                EventsContext.Instance.descriptionShowRequest?.Invoke(this, true);
            }

            if (Input.GetKeyUp(KeyCode.LeftAlt)) OnMouseExit();
        }

        void OnDestroy() {
            OnMouseExit();
            EventsContext.Instance.descriptionCreateRequest?.Invoke(this, false);
        }

        void OnMouseOver() {
            if (MouseContext.Instance.BlockedByUI) return;
            EventsContext.Instance.descriptionShowRequest?.Invoke(this, true);
        }

        void OnMouseExit() {
            EventsContext.Instance.descriptionShowRequest?.Invoke(this, false);
        }

        public void SetItem(Characters.Items.Item item) {
            Item = item;
            EventsContext.Instance.descriptionCreateRequest?.Invoke(this, true);
            Instantiate(item.Config.modelPrefab, transform);
            OnPickupError();
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
            if (!TryGetComponent(out Rigidbody rb)) return;
            rb.AddForce(Vector3.up * throwForce, ForceMode.Impulse);
            rb.AddTorque(new Vector3(VectorRandom.Range(throwTorqueRange),
                VectorRandom.Range(throwTorqueRange),
                VectorRandom.Range(throwTorqueRange)));
        }

        public Transform GetTransform() => transform;
    }
}