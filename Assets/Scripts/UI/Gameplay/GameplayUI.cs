using System;
using System.Collections;
using Cinemachine;
using DG.Tweening;
using Sirenix.OdinInspector;

using Unbowed.Gameplay;
using Unbowed.Gameplay.Characters;
using Unbowed.SO;
using Unbowed.UI.Gameplay.Inventory;
using Unbowed.UI.ItemNameplates;
using Unbowed.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace Unbowed.UI {
    [RequireComponent(typeof(Canvas))]
    public class GameplayUI : MonoBehaviour {
        [Header("Values")]
        [SerializeField] float screenAnimationTime;

        [Header("Whole screens")]
        [SerializeField] DroppedItemsContainerUI droppedItemsContainerUI;

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

        [Header("Dependencies")]
        [SerializeField] CinemachineVirtualCamera virtualCamera;

        CinemachineFramingTransposer _transposer;
        float _currentTransposerTargetValue;

        void Awake() {
            characterButton.onClick.AddListener(characterMenu.ToggleOpened);
            inventoryButton.onClick.AddListener(inventoryMenu.ToggleOpened);

            deathScreen.SetActive(false);
            pauseScreen.SetActive(false);
            characterMenu.Close();
            inventoryMenu.Close();

            _transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            _currentTransposerTargetValue = _transposer.m_ScreenX;

            ActivePlayer.PlayerChanged += SetInventory;
            inventoryMenu.SetInventory(ActivePlayer.GetInventory());

            leftMenus.IsOpened.Changed += (value) => FloatCameraLeft();
            rightMenus.IsOpened.Changed += (value) => FloatCameraRight();

            EventsContext.Instance.otherInventoryRequest += OnOtherInventoryRequest;
        }

        void OnDestroy() {
            ActivePlayer.PlayerChanged -= SetInventory;
            EventsContext.Instance.otherInventoryRequest -= OnOtherInventoryRequest;
        }

        void SetInventory() => inventoryMenu.SetInventory(ActivePlayer.GetInventory());

        void OnOtherInventoryRequest(Unbowed.Gameplay.Characters.Modules.Inventory inventory, bool value) {
            if (value) {
                lootedInventoryMenu.SetInventory(inventory);
                lootedInventoryMenu.Open();
            } else {
                if (lootedInventoryMenu.Inventory != inventory) return;
                lootedInventoryMenu.SetInventory(null);
                lootedInventoryMenu.Close();
            }
        }

        void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) pauseScreen.ToggleActive();
            if (Input.GetKeyDown(KeyCode.C)) characterMenu.ToggleOpened();
            if (Input.GetKeyDown(KeyCode.B)) inventoryMenu.ToggleOpened();
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