using UnityEngine;

namespace Unbowed.Utility {
    public class GameObjectSelector : MonoBehaviour {
        [SerializeField] private GameObject[] objects;

        private int currentObjectIndex = -1;

        private void Start() {
            if (objects.Length == 0) {
                Debug.LogError($"{gameObject} doesn't have anything to select.");
                enabled = false;
                return;
            }

            for (int i = 0; i < objects.Length; i++) {
                if (currentObjectIndex < 0 && objects[i].gameObject.activeSelf) {
                    currentObjectIndex = i;
                } else if (currentObjectIndex >= 0 && objects[i].gameObject.activeSelf) {
                    objects[i].gameObject.SetActive(false);
                }
            }

            if (currentObjectIndex < 0) {
                currentObjectIndex = 0;
                objects[currentObjectIndex].gameObject.SetActive(true);
            }
        }

        public void Step(bool isUp) {
            Debug.Log($"Stepping {isUp}");
            
            if (!isUp && currentObjectIndex == 0 || isUp && currentObjectIndex == objects.Length - 1) {
                return;
            }

            objects[currentObjectIndex].gameObject.SetActive(false);
            currentObjectIndex += isUp ? 1 : -1;
            objects[currentObjectIndex].gameObject.SetActive(true);
        }
    }
}