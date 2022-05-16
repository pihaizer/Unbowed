using System;
using System.Collections;
using Cinemachine;
using DG.Tweening;
using Sirenix.OdinInspector;

using Unbowed.Gameplay;
using Unbowed.Gameplay.Characters;
using Unbowed.Signals;
using Unbowed.SO;
using Unbowed.UI.Gameplay.Inventory;
using Unbowed.UI.ItemNameplates;
using Unbowed.Utility;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Unbowed.UI {
    [RequireComponent(typeof(Canvas))]
    public class GameplayUI : MonoBehaviour {
        [Header("Values")]
        [SerializeField]
        private float screenAnimationTime;

        [Header("Whole screens")]
        [SerializeField]
        private DroppedItemsContainerUI droppedItemsContainerUI;

        [SerializeField] private GameObject pauseScreen;

        [SerializeField] private GameObject deathScreen;

        [Header("PartialScreens")]
        [Title("Left")]
        [SerializeField]
        private MenuContainer leftMenus;

        [SerializeField] private Menu characterMenu;

        [SerializeField] private BagsUI lootedInventoryMenu;

        [Title("Right")]
        [SerializeField]
        private MenuContainer rightMenus;

        [SerializeField] private CharacterInventoryUI inventoryMenu;

        [Header("Menu buttons")]
        [SerializeField]
        private Button characterButton;

        [SerializeField] private Button inventoryButton;

        [Header("Dependencies")]
        [SerializeField]
        private CinemachineVirtualCamera virtualCamera;

        [Inject] private SignalBus _bus;
        
        private CinemachineFramingTransposer _transposer;
        private float _currentTransposerTargetValue;

        private void Awake() {
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

            leftMenus.IsOpened.Changed += _ => FloatCameraLeft();
            rightMenus.IsOpened.Changed += _ => FloatCameraRight();

            _bus.Subscribe<ShowInventoryRequest>(OnOtherInventoryRequest);
        }

        private void OnDestroy() {
            ActivePlayer.PlayerChanged -= SetInventory;
            _bus.Unsubscribe<ShowInventoryRequest>(OnOtherInventoryRequest);
        }

        private void SetInventory() => inventoryMenu.SetInventory(ActivePlayer.GetInventory());

        private void OnOtherInventoryRequest(ShowInventoryRequest signal) {
            if (signal.IsOpen) {
                lootedInventoryMenu.SetInventory(signal.Inventory);
                lootedInventoryMenu.Open();
            } else {
                if (lootedInventoryMenu.Inventory != signal.Inventory) return;
                lootedInventoryMenu.SetInventory(null);
                lootedInventoryMenu.Close();
            }
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) pauseScreen.ToggleActive();
            if (Input.GetKeyDown(KeyCode.C)) characterMenu.ToggleOpened();
            if (Input.GetKeyDown(KeyCode.B)) inventoryMenu.ToggleOpened();
        }

        private void FloatCameraLeft() {
            FloatScreen(leftMenus);
        }

        private void FloatCameraRight() {
            FloatScreen(rightMenus);
        }

        private void FloatScreen(Menu screen) {
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