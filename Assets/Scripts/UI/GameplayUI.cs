using System;
using System.Collections;
using System.Collections.Generic;
using SO.Events;
using UnityEngine;

public class GameplayUI : MonoBehaviour {
    [SerializeField] GameObject pauseScreen;
    [SerializeField] GameObject deathScreen;

    [Header("Events")]
    [SerializeField] EventSO deathEvent;

    void Start() {
        deathEvent.AddListener(OpenDeathScreen);
        
        deathScreen.SetActive(false);
        pauseScreen.SetActive(false);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            pauseScreen.SetActive(!pauseScreen.activeSelf);
        }
    }

    void OpenDeathScreen() {
        deathScreen.gameObject.SetActive(true);
    }
}
