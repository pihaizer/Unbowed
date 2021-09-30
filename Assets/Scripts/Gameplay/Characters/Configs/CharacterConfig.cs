using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Unbowed.Gameplay.Characters.Configs.Stats;
using UnityEngine;

namespace Unbowed.Gameplay.Characters.Configs {
    [CreateAssetMenu]
    public class CharacterConfig : SerializedScriptableObject {
        [NonSerialized, OdinSerialize]
        [HorizontalGroup]
        [Title("Base stats", TitleAlignment = TitleAlignments.Centered)]
        public CharacterStats baseStats = new CharacterStats();

        [Title("Base stats", TitleAlignment = TitleAlignments.Centered)]
        public CharacterDistances distances;

        [Title("Animation config")]
        public CharacterAnimationConfig animationConfig;
        
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
        [HorizontalGroup]
        [Title("Runtime stats preview", TitleAlignment = TitleAlignments.Centered)]
        CharacterRuntimeStats runtimeStatsPreview;

        #endregion
    }
}