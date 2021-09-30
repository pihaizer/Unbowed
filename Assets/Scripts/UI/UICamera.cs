using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR

#endif

namespace Unbowed.UI {
    [RequireComponent(typeof(Camera)), ExecuteInEditMode]
    public class UICamera : MonoBehaviour {
        [SerializeField] Side side;

        void OnEnable() {
            var uiCamera = GetComponent<Camera>();
            var uiCameraRect = uiCamera.rect;
            int screenWidth = 4, screenHeight = 3;
#if UNITY_EDITOR
            string[] res = UnityStats.screenRes.Split('x');
            screenWidth = int.Parse(res[0]);
            screenHeight = int.Parse(res[1]);
#else
            screenWidth = Screen.width;
            screenHeight = Screen.height;
#endif
            Debug.Log($"{screenWidth} : {screenHeight}");
            uiCameraRect.width = (float) screenHeight / screenWidth * 2f / 3f;
            uiCameraRect.x = side == Side.Left ? 0 : 1 - uiCameraRect.width;

            var mainCamera = Camera.main;
            var mainCameraRect = mainCamera.rect;

            mainCameraRect.width -= uiCameraRect.width;

            if (side == Side.Left) {
                mainCameraRect.x += uiCameraRect.width;
            }

            uiCamera.rect = uiCameraRect;
            mainCamera.rect = mainCameraRect;
        }

        void OnDisable() {
            var uiCamera = GetComponent<Camera>();
            var uiCameraRect = uiCamera.rect;
            var mainCamera = Camera.main;
            var mainCameraRect = mainCamera.rect;

            mainCameraRect.width += uiCameraRect.width;

            if (side == Side.Left) {
                mainCameraRect.x = 0;
            }

            mainCamera.rect = mainCameraRect;
        }

        enum Side {
            Left,
            Right
        }
    }
}