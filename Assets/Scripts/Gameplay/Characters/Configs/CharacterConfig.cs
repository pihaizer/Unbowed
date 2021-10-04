using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Unbowed.Gameplay.Characters.Configs.Stats;
using UnityEngine;

namespace Unbowed.Gameplay.Characters.Configs {
    [CreateAssetMenu]
    public class CharacterConfig : SerializedScriptableObject {
        [NonSerialized, OdinSerialize]
        [HorizontalGroup("Stats"), PropertyOrder(0)]
        [Title("Base stats", TitleAlignment = TitleAlignments.Centered)]
        public CharacterStats baseStats = new CharacterStats();

        [VerticalGroup("Stats/Other"), PropertyOrder(2)]
        [Title("Distances", TitleAlignment = TitleAlignments.Centered)]
        public CharacterDistances distances;

        [VerticalGroup("Stats/Other"), PropertyOrder(3)]
        public CharacterAnimationConfig animationConfig;

        [VerticalGroup("Stats/Other"), PropertyOrder(4)]
        public DropsConfig dropsConfig;
        
        #region EDITOR_ONLY

        void OnEnable() {
            if (Application.isEditor) CalculatePreview();
        }

        [Button, PropertyOrder(2)]
        void CalculatePreview() {
            if (runtimeStatsPreview == null)
                runtimeStatsPreview = new CharacterRuntimeStats(baseStats);
            else
                runtimeStatsPreview.SetBaseStats(baseStats);
        }

        [NonSerialized, ShowInInspector]
        [HorizontalGroup("Stats"), PropertyOrder(1)]
        [Title("Runtime stats preview", TitleAlignment = TitleAlignments.Centered)]
        CharacterRuntimeStats runtimeStatsPreview;

        #endregion
    }
}