using Sirenix.OdinInspector;
using Unbowed.Gameplay.Characters.Items.Configs;
using Unbowed.Gameplay.Characters.Modules;
using Unbowed.Signals;
using Unbowed.SO;
using Unbowed.Utility;
using UnityEngine;
using Zenject;

namespace Unbowed.Gameplay.Items {
    public class DroppedItem : MonoBehaviour, IInteractable {
        [SerializeField, Range(0, 20f)] private float throwForce = 1f;
        [SerializeField, MinMaxSlider(0, 20f)] private Vector2 throwTorqueRange;
        [SerializeField, AssetsOnly] private GameObject defaultItemModel;

        [Inject] private SignalBus _bus;

        public Characters.Items.Item Item { get; private set; }

        private void Update() {
            if (Input.GetKey(KeyCode.LeftAlt)) {
                if (!RectUtils.One.Contains(Camera.main.WorldToViewportPoint(transform.position))) return;
                _bus.Fire(new DescriptionShowRequestSignal(this, true));
            }

            if (Input.GetKeyUp(KeyCode.LeftAlt)) OnMouseExit();
        }

        private void OnDestroy() {
            OnMouseExit();
            _bus.Fire(new DescriptionCreateRequestSignal(this, false));
        }

        private void OnMouseOver() {
            if (MouseContext.BlockedByUI) return;
            _bus.Fire(new DescriptionShowRequestSignal(this, true));
        }

        private void OnMouseExit() {
            _bus.Fire(new DescriptionShowRequestSignal(this, false));
        }

        public void SetItem(Characters.Items.Item item) {
            Item = item;
            // _bus.Fire(new DescriptionCreateRequestSignal(this, true));
            Instantiate(item.Config.modelPrefab != null ? 
                item.Config.modelPrefab :
                defaultItemModel, transform);
            ThrowItemModelUpwards();
        }

        public void Interact(GameObject source) {
            if (!source.TryGetComponent(out Inventory inventory)) return;
            if (inventory.TryAddItemToBags(Item)) {
                Destroy(gameObject);
            } else {
                ThrowItemModelUpwards();
            }
        }

        [Button]
        private void ThrowItemModelUpwards() {
            if (!TryGetComponent(out Rigidbody rb)) return;
            rb.AddForce(Vector3.up * throwForce, ForceMode.Impulse);
            rb.AddTorque(new Vector3(VectorRandom.Range(throwTorqueRange),
                VectorRandom.Range(throwTorqueRange),
                VectorRandom.Range(throwTorqueRange)));
        }

        public Transform GetTransform() => transform;
    }
}