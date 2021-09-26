using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour {
    public void ScheduleRespawn(Mortal mortal, float delay) =>
        StartCoroutine(RespawnCoroutine(mortal, delay));

    static IEnumerator RespawnCoroutine(Mortal mortal, float delay) {
        yield return new WaitForSeconds(delay);
        mortal.Revive();
    }
}