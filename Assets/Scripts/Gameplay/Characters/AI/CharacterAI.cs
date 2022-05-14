﻿using System;
using System.Collections;
using Unbowed.Gameplay.Characters.AI.Brains;
using Unbowed.SO.Brains;
using UnityEngine;

namespace Unbowed.Gameplay.Characters.AI {
    [RequireComponent(typeof(Character))]
    public class CharacterAI : MonoBehaviour {
        [SerializeField] private BrainConfigSO brainConfig;
        [SerializeField] private ColliderZone restrictedZone;

        private Brain _brain;

        private IEnumerator Start() {
            var character = GetComponent<Character>();
            yield return new WaitUntil(() => character.IsStarted);
            if (brainConfig) {
                _brain = brainConfig.Inject(character);
                if (restrictedZone) _brain.SetRestrictedZone(restrictedZone);
            }
        }

        private void FixedUpdate() => _brain?.FixedUpdate();

        private void Update() => _brain?.Update();

        private void OnDestroy() => _brain?.OnDestroy();

        private void OnValidate() {
            if (!Application.isPlaying) return;
            var character = GetComponent<Character>();
            if (!character.IsStarted) return;
            if (brainConfig == null) {
                _brain = null;
            } else if (_brain?.ID != brainConfig.ID) {
                _brain?.OnDestroy();
                _brain = brainConfig.Inject(GetComponent<Character>());
            }
        }
    }
}