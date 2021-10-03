using System.Collections;
using Unbowed.Gameplay.Characters.Modules;
using UnityEngine;

namespace Unbowed.Gameplay {
    public class Respawner : MonoBehaviour {
        public void ScheduleRespawn(Health health, float delay) =>
            StartCoroutine(RespawnCoroutine(health, delay));

        static IEnumerator RespawnCoroutine(Health health, float delay) {
            yield return new WaitForSeconds(delay);
            health.Revive();
        }
    }
}