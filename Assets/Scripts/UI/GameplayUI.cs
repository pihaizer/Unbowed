using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using Sirenix.OdinInspector;
using Unbowed.Gameplay.Characters.Items;
using Unbowed.SO;
using Unbowed.SO.Events;
using Unbowed.UI.Inventory;
using Unbowed.Utility;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Unbowed.UI {
    [RequireComponent(typeof(Canvas))]
    public class GameplayUI : MonoBehaviour {
        [Header("Values")]
        [SerializeField] float screenAnimationTime;

        [Header("Whole screens")]
        [SerializeField] GameObject pauseScreen;

        [SerializeField] GameObject deathScreen;

        [Header("PartialScreens")]
        [Title("Left")]
        [SerializeField] MenuContainer leftMenus;

        [SerializeField] Menu characterMenu;
        
        [SerializeField] BagsUI lootedInventoryMenu;

        [Title("Right")]
        [SerializeField] MenuContainer rightMenus;

        [SerializeField] CharacterInventoryUI inventoryMenu;

        [Header("Menu buttons")]
        [SerializeField] Button characterButton;

        [SerializeField] Button inventoryButton;

        [Header("Events")]
        [SerializeField] EventSO deathEvent;

        [Header("Dependencies")]
        [SerializeField] CinemachineVirtualCamera virtualCamera;

        CinemachineFramingTransposer _transposer;
        float _currentTransposerTargetValue;

        static Gameplay.Characters.Modules.Inventory PlayerInventory =>
            GlobalContext.Instance.playerCharacter.inventory;

        IEnumerator Start() {
            deathEvent.AddListener(OpenDeathScreen);

            characterButton.onClick.AddListener(characterMenu.ToggleOpened);
            inventoryButton.onClick.AddListener(inventoryMenu.ToggleOpened);

            deathScreen.SetActive(false);
            pauseScreen.SetActive(false);
            characterMenu.Close();
            inventoryMenu.Close();

            _transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            _currentTransposerTargetValue = _transposer.m_ScreenX;

            yield return new WaitUntil(() => GlobalContext.Instance.playerCharacter &&
                                             GlobalContext.Instance.playerCharacter.IsStarted);

            inventoryMenu.SetInventory(PlayerInventory);
            leftMenus.IsOpened.Changed += (value) => FloatCameraLeft();
            rightMenus.IsOpened.Changed += (value) => FloatCameraRight();
            
            GlobalContext.Instance.OpenOtherInventoryRequest += OnOpenOtherInventoryRequest;
            GlobalContext.Instance.CloseOtherInventoryRequest += CloseOtherInventoryRequest;
        }

        void OnOpenOtherInventoryRequest(Gameplay.Characters.Modules.Inventory inventory) {
            lootedInventoryMenu.SetInventory(inventory);
            lootedInventoryMenu.Open();
        }

        void CloseOtherInventoryRequest(Gameplay.Characters.Modules.Inventory inventory) {
            if (lootedInventoryMenu.Inventory != inventory) return;
            lootedInventoryMenu.SetInventory(null);
            lootedInventoryMenu.Close();
        }

        void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) pauseScreen.ToggleActive();
            if (Input.GetKeyDown(KeyCode.C)) characterMenu.ToggleOpened();
            if (Input.GetKeyDown(KeyCode.B)) inventoryMenu.ToggleOpened();
        }

        void OpenDeathScreen() {
            deathScreen.gameObject.SetActive(true);
        }

        void FloatCameraLeft() {
            FloatScreen(leftMenus);
        }

        void FloatCameraRight() {
            FloatScreen(rightMenus);
        }

        void FloatScreen(Menu screen) {
            var rectTransform = screen.GetComponent<RectTransform>();
            var cam = Camera.main;
            float offsetPixels = rectTransform.sizeDelta.x * transform.localScale.x *
                                 (screen.IsOpened ? 1 : -1) *
                                 (rectTransform.anchorMin.x < 0.5 ? 1 : -1);
            _currentTransposerTargetValue += offsetPixels / cam.pixelWidth / 2;
            DOTween.To(() => _transposer.m_ScreenX, value => _transposer.m_ScreenX = value,
                _currentTransposerTargetValue, screenAnimationTime);
        }
    }
}