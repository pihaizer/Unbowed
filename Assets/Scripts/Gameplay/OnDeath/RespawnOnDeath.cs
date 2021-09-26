using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Gameplay.OnDeath {
    [RequireComponent(typeof(Mortal))]
    public class RespawnOnDeath : MonoBehaviour {
        [SerializeField] AnimationClip optionalAnimationDelay;
        [SerializeField] Respawner respawner;
        [SerializeField] Vector2 respawnDelayRange;

        void Start() {
            if (respawner == null) {
                Debug.LogWarning("Respawner is not set.");
            }

            GetComponent<Mortal>().isDead.Changed += value => {
                if (value) StartCoroutine(RespawnCoroutine());
            };
        }

        IEnumerator RespawnCoroutine() {
            yield return new WaitForOptionalAnimation(optionalAnimationDelay);
            gameObject.SetActive(false);
            if (respawner != null) {
                respawner.ScheduleRespawn(GetComponent<Mortal>(),
                    Random.Range(respawnDelayRange.x, respawnDelayRange.y));
            } else {
                Debug.LogWarning("Tried to respawn, but respawner was null.");
            }
        }
    }
}