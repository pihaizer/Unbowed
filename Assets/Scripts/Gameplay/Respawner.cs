using System.Collections;
using UnityEngine;

namespace Unbowed.Gameplay {
    public class Respawner : MonoBehaviour {
        public void ScheduleRespawn(HealthModule healthModule, float delay) =>
            StartCoroutine(RespawnCoroutine(healthModule, delay));

        static IEnumerator RespawnCoroutine(HealthModule healthModule, float delay) {
            yield return new WaitForSeconds(delay);
            healthModule.Revive();
        }
    }
}