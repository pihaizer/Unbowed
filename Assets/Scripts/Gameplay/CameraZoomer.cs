using Cinemachine;
using DG.Tweening;
using UnityEngine;

namespace Unbowed.Gameplay {
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraZoomer : MonoBehaviour {
        [SerializeField] private float[] distances;
        [SerializeField] private float zoomTime = 0.75f;
        [SerializeField] private int currentDistanceIndex;

        private CinemachineFramingTransposer _transposer;

        private void Start() {
            if (distances.Length == 0) {
                Debug.LogError($"{gameObject} doesn't have anything to select.");
                enabled = false;
                return;
            }

            var virtualCamera = GetComponent<CinemachineVirtualCamera>();
            _transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.RightBracket)) Step(true);
            else if (Input.GetKeyDown(KeyCode.LeftBracket)) Step(false);
        }

        public void Step(bool isUp) {
            if (!isUp && currentDistanceIndex == 0 || isUp && currentDistanceIndex == distances.Length - 1) {
                return;
            }

            currentDistanceIndex += isUp ? 1 : -1;

            DOTween.To(() => _transposer.m_CameraDistance,
                value => _transposer.m_CameraDistance = value,
                distances[currentDistanceIndex], zoomTime);
        }
    }
}