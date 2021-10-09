using System;
using Cinemachine;
using Unbowed.SO;
using UnityEngine;

namespace Unbowed.Gameplay {
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class PlayerCamera : MonoBehaviour {
        CinemachineVirtualCamera _camera;
        
        void Awake() {
            _camera = GetComponent<CinemachineVirtualCamera>();
            ActivePlayer.PlayerChanged += ActivePlayerOnPlayerChanged;
            ActivePlayerOnPlayerChanged();
        }

        void ActivePlayerOnPlayerChanged() {
            if (!ActivePlayer.Exists) return;
            var playerTransform = ActivePlayer.GetTransform();
            _camera.Follow = playerTransform;
        }
    }
}