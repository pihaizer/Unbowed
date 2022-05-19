using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using Unbowed.Gameplay.Characters.Configs.Stats;
using Unbowed.Gameplay.Characters.Items;
using Unbowed.Utility;
using UnityEngine;
using UnityEngine.UI;
using Item = Unbowed.Gameplay.Characters.Items.Item;

namespace Unbowed.UI.Gameplay.Items
{
    public class ItemDescriptionUI : GameObjectScreen
    {
        [SerializeField, ChildGameObjectsOnly] private TMP_Text itemName;
        [SerializeField, ChildGameObjectsOnly] private TMP_Text equipmentType;
        [SerializeField, ChildGameObjectsOnly] private TMP_Text primaryEffectorsText;
        [SerializeField, ChildGameObjectsOnly] private TMP_Text secondaryEffectorsText;
        [SerializeField, ChildGameObjectsOnly] private TMP_Text description;
        [SerializeField] private Vector2 screenMaxOffset;

        public void SetItem(Item item)
        {
            if (item == null) return;

            itemName.text = item.Config.displayName;
            itemName.color = new Color(item.Color.r, item.Color.g, item.Color.b, 1);

            description.gameObject.SetActive(!string.IsNullOrEmpty(item.Config.description));
            description.SetText(item.Config.description);

            primaryEffectorsText.text = "";
            secondaryEffectorsText.text = "";

            if (item is not Equipment {Stats: { }} equipment)
            {
                equipmentType.gameObject.SetActive(false);
                return;
            }

            foreach (StatEffector modifier in equipment.Stats.statEffectors)
            {
                if (modifier.isPrimary)
                    primaryEffectorsText.text += modifier.GetDescription() + '\n';
                else
                    secondaryEffectorsText.text += modifier.GetDescription() + '\n';
            }

            equipmentType.gameObject.SetActive(true);
            equipmentType.text = equipment.EquipmentTypeName;
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        }

        public void AlignNear(RectTransform targetRt)
        {
            var rt = GetComponent<RectTransform>();
            rt.position = targetRt.position - Vector3.Scale(targetRt.pivot, targetRt.GetProperSize())
                          + Vector3.Scale(targetRt.GetProperSize(), new Vector2(0.5f, 1));
            rt.pivot = new Vector2(0.5f, 0);
            rt.ForceUpdateRectTransforms();

            Canvas canvas = GetComponentsInParent<Canvas>().First(parentCanvas => parentCanvas.isRootCanvas);
            float scaleFactor = canvas.scaleFactor;

            if (rt.position.x - rt.pivot.x * rt.GetProperSize().x * scaleFactor < 0)
            {
                rt.position = new Vector2(rt.pivot.x * rt.GetProperSize().x * scaleFactor + screenMaxOffset.x,
                    rt.position.y);
            }

            var max = (Vector2) rt.position + new Vector2(rt.GetProperSize().x * rt.pivot.x, rt.GetProperSize().y)
                * scaleFactor + screenMaxOffset;

            if (canvas.pixelRect.Contains(max)) return;

            Vector2 newPos = rt.position;

            if (max.x > canvas.renderingDisplaySize.x)
            {
                newPos.x = canvas.renderingDisplaySize.x -
                           rt.pivot.x * rt.GetProperSize().x * scaleFactor - screenMaxOffset.x;
            }

            if (max.y > canvas.renderingDisplaySize.y)
            {
                rt.pivot = new Vector2(0.5f, 1);
                rt.position = targetRt.position - Vector3.Scale(targetRt.pivot, targetRt.GetProperSize())
                              + Vector3.Scale(targetRt.GetProperSize(), new Vector2(0.5f, 0));
                rt.ForceUpdateRectTransforms();
                newPos.y = rt.position.y;

                if (newPos.y - rt.GetProperSize().y < screenMaxOffset.y)
                    newPos.y = screenMaxOffset.y + rt.GetProperSize().y;
            }

            rt.position = newPos;
            rt.ForceUpdateRectTransforms();
        }
    }
}