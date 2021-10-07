using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unbowed.Gameplay.Characters.Items;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Unbowed.Gameplay.Characters.Configs {
    [Serializable]
    [HideLabel]
    [InlineProperty(LabelWidth = 150)]
    public class DropsConfig {
        public bool hasDrops = true;


        [ShowIf(nameof(hasDrops)), GUIColor(nameof(GetValueColor))]
        public AnimationCurve valueCurve;

        [ShowIf(nameof(hasDrops)), GUIColor(nameof(GetAmountColor))]
        public AnimationCurve amountCurve;

        [ShowIf(nameof(hasDrops)), GUIColor(nameof(GetItemLevelColor))]
        public AnimationCurve itemLevelCurve;

        [ShowIf(nameof(hasDrops))]
        public List<Item> alwaysDrops;

        Color GetValueColor() => Color.Lerp(Color.white, Color.yellow,
            (valueCurve.Evaluate(0) + valueCurve.Evaluate(1)) / 200f);
        
        Color GetAmountColor() => Color.Lerp(Color.white, Color.green, 
            (amountCurve.Evaluate(0) + amountCurve.Evaluate(1)) / 200f);
        
        Color GetItemLevelColor() => Color.Lerp(Color.white, Color.blue,
            (itemLevelCurve.Evaluate(0) + itemLevelCurve.Evaluate(1)) / 200f);
        

        [Button]
        void SimulateDrops() {
            
        }
    }
}