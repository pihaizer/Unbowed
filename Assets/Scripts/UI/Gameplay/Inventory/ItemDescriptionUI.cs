using System;
using System.Collections;
using System.Linq;

using Sirenix.OdinInspector;

using TMPro;

using Unbowed.Gameplay.Items;

using UnityEngine;
using UnityEngine.Serialization;

using Item = Unbowed.Gameplay.Characters.Items.Item;

namespace Unbowed.UI.Gameplay.Inventory {
    public class ItemDescriptionUI : CanvasGroupMenu {
        [SerializeField, ChildGameObjectsOnly] TMP_Text itemName;
        [FormerlySerializedAs("slot"), SerializeField, ChildGameObjectsOnly]
        TMP_Text equipmentType;
        [SerializeField, ChildGameObjectsOnly] TMP_Text primaryEffectorsText;
        [SerializeField, ChildGameObjectsOnly] TMP_Text secondaryEffectorsText;
        [SerializeField, ChildGameObjectsOnly] TMP_Text description;
        [SerializeField] Vector2 screenMaxOffset;

        public void SetItem(Item item) {
            if (item == null) {
                Close();
                return;
            }

            itemName.text = item.Name;
            itemName.color = new Color(item.Color.r, item.Color.g, item.Color.b, 1);

            description.gameObject.SetActive(!string.IsNullOrEmpty(item.Config.description));
            description.SetText(item.Config.description);

            primaryEffectorsText.text = "";
            secondaryEffectorsText.text = "";
            if (item.statEffectorsBundle != null) {
               
                foreach (var modifier in item.statEffectorsBundle.statModifiers) {
                    if (modifier.isPrimary)
                        primaryEffectorsText.text += modifier.GetDescription() + '\n';
                    else
                        secondaryEffectorsText.text += modifier.GetDescription() + '\n';
                }
            }

            equipmentType.gameObject.SetActive(item.IsEquipment());

            if (item.IsEquipment(out var equipmentConfig)) {
                equipmentType.text = equipmentConfig.type.ToString();
            }
        }

        protected override void SetOpened(bool value) {
            if (value) UpdatePosition();
            base.SetOpened(value);
        }

        void UpdatePosition() {
            var rt = GetComponent<RectTransform>();
            rt.pivot = new Vector2(0.5f, 0);
            rt.anchorMax = rt.anchorMin = new Vector2(0.5f, 1);
            rt.anchoredPosition = Vector2.zero;
            rt.ForceUpdateRectTransforms();

            var canvas = GetComponentsInParent<Canvas>().First(parentCanvas => parentCanvas.isRootCanvas);
            float scaleFactor = canvas.scaleFactor;

            if (rt.position.x - rt.pivot.x * rt.sizeDelta.x * scaleFactor < 0) {
                rt.position = new Vector2(rt.pivot.x * rt.sizeDelta.x * scaleFactor + screenMaxOffset.x,
                    rt.position.y);
            }

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

                if (newPos.y - rt.sizeDelta.y < screenMaxOffset.y) newPos.y = screenMaxOffset.y + rt.sizeDelta.y;
            }

            rt.position = newPos;
            rt.ForceUpdateRectTransforms();
        }
    }
}