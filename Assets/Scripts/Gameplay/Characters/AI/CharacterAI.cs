using System.Collections;
using Unbowed.Gameplay.Characters.AI.Brains;
using Unbowed.SO.Brains;
using UnityEngine;

namespace Unbowed.Gameplay.Characters.AI {
    [RequireComponent(typeof(Character))]
    public class CharacterAI : MonoBehaviour {
        [SerializeField] BrainConfigSO brainConfig;
        [SerializeField] ColliderZone restrictedZone;

        Brain _brain;

        IEnumerator Start() {
            var character = GetComponent<Character>();
            yield return new WaitUntil(() => character.IsStarted);
            if (brainConfig) {
                _brain = brainConfig.Inject(character);
                if (restrictedZone) _brain.SetRestrictedZone(restrictedZone);
            }
        }

        void FixedUpdate() {
            _brain?.Update(Time.fixedDeltaTime);
        }

        void OnValidate() {
            if (!Application.isPlaying) return;
            if (brainConfig == null) {
                _brain = null;
            } else if (_brain?.ID != brainConfig.ID) {
                _brain = brainConfig.Inject(GetComponent<Character>());
            }
        }
    }
}