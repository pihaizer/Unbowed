using System.Collections;
using Unbowed.Gameplay.Characters;
using Unbowed.Utility.WaitFor;
using UnityEngine;

namespace Unbowed.Gameplay.OnDeath {
    [RequireComponent(typeof(Character))]
    public class RespawnOnDeath : MonoBehaviour {
        [SerializeField] AnimationClip optionalAnimationDelay;
        [SerializeField] Respawner respawner;
        [SerializeField] Vector2 respawnDelayRange;

        void Start() {
            if (respawner == null) {
                Debug.LogWarning("Respawner is not set.");
            }

            GetComponent<Character>().Health.isDead.Changed += value => {
                if (value) StartCoroutine(RespawnCoroutine());
            };
        }

        IEnumerator RespawnCoroutine() {
            yield return new WaitForOptionalAnimation(optionalAnimationDelay);
            gameObject.SetActive(false);
            if (respawner != null) {
                respawner.ScheduleRespawn(GetComponent<Character>().Health,
                    Random.Range(respawnDelayRange.x, respawnDelayRange.y));
            } else {
                Debug.LogWarning("Tried to respawn, but respawner was null.");
            }
        }
    }
}