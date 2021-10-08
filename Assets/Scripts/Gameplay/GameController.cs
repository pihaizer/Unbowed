using System;
using Unbowed.SO;
using UnityEngine;

namespace Unbowed.Gameplay {
    public class GameController : MonoBehaviour {
        [SerializeField] SceneConfig startingLocationConfig;

        void Start() {
            ScenesConfig.Instance.Load(new SceneChangeRequest(startingLocationConfig) {useLoadingScreen = true});
        }
    }
}