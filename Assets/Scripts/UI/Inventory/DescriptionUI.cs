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

            var rectTransform = GetComponent<RectTransform>();
            rectTransform.anchoredPosition = Vector2.zero;

            var canvas = GetComponentInParent<Canvas>();
            float scaleFactor = canvas.scaleFactor;

            var max = (Vector2)rectTransform.position + rectTransform.sizeDelta * scaleFactor;
            if (canvas.pixelRect.Contains(max)) return;

            Vector2 newPos = rectTransform.position;

            if (max.x > canvas.renderingDisplaySize.x) {
                newPos.x = canvas.pixelRect.width - rectTransform.rect.width * scaleFactor;
            }

            if (max.y > canvas.renderingDisplaySize.y) {
                newPos.y = canvas.pixelRect.height - rectTransform.rect.height * scaleFactor;
            }

            Debug.Log($"new pos {newPos}");

            rectTransform.position = newPos;
        }
    }
}