using System.Collections;
using Unbowed.Gameplay.Characters;
using Unbowed.Utility.WaitFor;
using UnityEngine;

namespace Unbowed.Gameplay.OnDeath {
    [RequireComponent(typeof(Character))]
    public class RespawnOnDeath : MonoBehaviour {
        [SerializeField] private AnimationClip optionalAnimationDelay;
        [SerializeField] private Respawner respawner;
        [SerializeField] private Vector2 respawnDelayRange;

        private void Start() {
            if (respawner == null) {
                Debug.LogWarning("Respawner is not set.");
            }

            GetComponent<Character>().health.Died += (data) => StartCoroutine(RespawnCoroutine());
        }

        private IEnumerator RespawnCoroutine() {
            yield return new WaitForOptionalAnimation(optionalAnimationDelay);
            gameObject.SetActive(false);
            if (respawner != null) {
                respawner.ScheduleRespawn(GetComponent<Character>().health,
                    Random.Range(respawnDelayRange.x, respawnDelayRange.y));
            } else {
                Debug.LogWarning("Tried to respawn, but respawner was null.");
            }
        }
    }
}