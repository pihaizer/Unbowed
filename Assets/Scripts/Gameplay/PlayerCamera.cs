using System;
using Cinemachine;
using Unbowed.SO;
using UnityEngine;

namespace Unbowed.Gameplay {
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class PlayerCamera : MonoBehaviour {
        private CinemachineVirtualCamera _camera;

        private void Awake() {
            _camera = GetComponent<CinemachineVirtualCamera>();
            ActivePlayer.PlayerChanged += ActivePlayerOnPlayerChanged;
            ActivePlayerOnPlayerChanged();
        }

        private void ActivePlayerOnPlayerChanged() {
            if (!ActivePlayer.Exists) return;
            var playerTransform = ActivePlayer.GetTransform();
            _camera.Follow = playerTransform;
        }
    }
}