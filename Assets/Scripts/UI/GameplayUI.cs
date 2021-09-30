using Cinemachine;
using DG.Tweening;
using Unbowed.SO.Events;
using Unbowed.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace Unbowed.UI {
    public class GameplayUI : MonoBehaviour {
        [Header("Values")]
        [SerializeField] float screenAnimationTime;
        
        [Header("Whole screens")]
        [SerializeField] GameObject pauseScreen;

        [SerializeField] GameObject deathScreen;

        [Header("PartialScreens")]
        [SerializeField] GameObject characterScreen;

        [SerializeField] GameObject inventoryScreen;

        [Header("Menu buttons")]
        [SerializeField] Button characterButton;

        [SerializeField] Button inventoryButton;

        [Header("Events")]
        [SerializeField] EventSO deathEvent;

        [Header("Dependencies")]
        [SerializeField] CinemachineVirtualCamera virtualCamera;

        CinemachineFramingTransposer _transposer;
        float _currentTransposerTargetValue;

        void Start() {
            deathEvent.AddListener(OpenDeathScreen);

            characterButton.onClick.AddListener(ToggleLeftMenus);
            inventoryButton.onClick.AddListener(ToggleRightMenus);

            deathScreen.SetActive(false);
            pauseScreen.SetActive(false);

            _transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            _currentTransposerTargetValue = _transposer.m_ScreenX;
        }

        void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) pauseScreen.ToggleActive();
            if (Input.GetKeyDown(KeyCode.C)) ToggleLeftMenus();
            if (Input.GetKeyDown(KeyCode.B)) ToggleRightMenus();
        }

        void OpenDeathScreen() {
            deathScreen.gameObject.SetActive(true);
        }

        void ToggleLeftMenus() {
            characterScreen.ToggleActive();
            
            var characterScreenRt = characterScreen.GetComponent<RectTransform>();

            FloatScreen(characterScreenRt);
        }

        void ToggleRightMenus() {
            inventoryScreen.ToggleActive();
            
            var inventoryScreenRt = inventoryScreen.GetComponent<RectTransform>();
            
            FloatScreen(inventoryScreenRt);
        }

        void FloatScreen(RectTransform screen) {
            var cam = Camera.main;
            float offsetPixels = screen.sizeDelta.x * transform.localScale.x * 
                                 (screen.gameObject.activeSelf ? 1 : -1) *
                                 (screen.anchorMin.x < 0.5 ? 1 : -1);
            _currentTransposerTargetValue += offsetPixels / cam.pixelWidth / 2;
            DOTween.To(() => _transposer.m_ScreenX, value => _transposer.m_ScreenX = value,
                _currentTransposerTargetValue, screenAnimationTime);
        }
    }
}