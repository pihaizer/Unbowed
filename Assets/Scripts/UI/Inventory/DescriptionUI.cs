using Sirenix.OdinInspector;
using TMPro;
using Unbowed.Gameplay.Characters.Items;
using UnityEngine;
using UnityEngine.UI;

namespace Unbowed.UI.Inventory {
    public class DescriptionUI : Menu {
        [SerializeField, ChildGameObjectsOnly] TMP_Text itemName;
        [SerializeField, ChildGameObjectsOnly] TMP_Text slot;
        [SerializeField, ChildGameObjectsOnly] TMP_Text description;
        [SerializeField] Vector2 screenMaxOffset;

        public void SetItem(Item item) {
            if (item == null) {
                Close();
                return;
            }

            itemName.text = item.Name;
            itemName.color = new Color(item.Color.r, item.Color.g, item.Color.b, 1);

            description.gameObject.SetActive(!string.IsNullOrEmpty(item.config.description));
            description.SetText(item.config.description);

            slot.gameObject.SetActive(item.IsEquipment);

            if (item.IsEquipment) {
                slot.text = item.Slot.ToString();
            }
        }

        public override void SetOpened(bool value) {
            base.SetOpened(value);
            if (!value) return;
            SetDescriptionPosition();
        }

        void SetDescriptionPosition() {
            var rt = GetComponent<RectTransform>();
            rt.pivot = new Vector2(0.5f, 0);
            rt.anchorMax = rt.anchorMin = new Vector2(0.5f, 1);
            rt.anchoredPosition = Vector2.zero;
            rt.ForceUpdateRectTransforms();

            var canvas = GetComponentInParent<Canvas>();
            float scaleFactor = canvas.scaleFactor;
            var max = (Vector2) rt.position + new Vector2(rt.sizeDelta.x * rt.pivot.x, rt.sizeDelta.y)
                * scaleFactor + screenMaxOffset;

            if (canvas.pixelRect.Contains(max)) return;

            Vector2 newPos = rt.position;

            if (max.x > canvas.renderingDisplaySize.x) {
                newPos.x = canvas.renderingDisplaySize.x - 
                           rt.pivot.x * rt.sizeDelta.x * scaleFactor - screenMaxOffset.x;
            }

            if (max.y > canvas.renderingDisplaySize.y) {
                rt.pivot = new Vector2(0.5f, 1);
                rt.anchorMax = rt.anchorMin = new Vector2(0.5f, 0);
                rt.anchoredPosition = Vector2.zero;
                rt.ForceUpdateRectTransforms();
                newPos.y = rt.position.y;

                if (newPos.y - rt.sizeDelta.y < screenMaxOffset.y)
                    newPos.y = screenMaxOffset.y + rt.sizeDelta.y;
            }
            
            rt.position = newPos;
            rt.ForceUpdateRectTransforms();
        }
    }
}