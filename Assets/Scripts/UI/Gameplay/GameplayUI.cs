using System;
using System.Collections;
using Cinemachine;
using DG.Tweening;
using HyperCore.UI;
using Sirenix.OdinInspector;
using Unbowed.Gameplay;
using Unbowed.Gameplay.Characters;
using Unbowed.Gameplay.Characters.Modules;
using Unbowed.SO;
using Unbowed.UI.Gameplay.Inventory;
using Unbowed.UI.Signals;
using Unbowed.Utility;
using UnityEngine;
using Zenject;

namespace Unbowed.UI
{
    [RequireComponent(typeof(Canvas))]
    public class GameplayUI : MonoBehaviour
    {
        [Header("Values")]
        [SerializeField] private float screenAnimationTime;

        [Header("PartialScreens")]
        [Title("Left")]
        [SerializeField] private CanvasScreenContainer leftCanvasScreens;

        // [SerializeField] private BagsUI lootedInventoryMenu;

        [Title("Right")]
        [SerializeField] private CanvasScreenContainer rightCanvasScreens;

        [SerializeField] private CharacterInventoryUI inventoryMenu;

        [Header("Camera")]
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private float sideScreensCameraOffset = 0.2f;
        [Inject] private SignalBus _bus;

        private CinemachineFramingTransposer _transposer;
        private float _defaultTransposerTargetValue;
        private float _currentTransposerTargetValue;

        private void Start()
        {
            leftCanvasScreens.CloseAll();
            rightCanvasScreens.CloseAll();

            _transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            _defaultTransposerTargetValue = _transposer.m_ScreenX;
            _currentTransposerTargetValue = _defaultTransposerTargetValue;

            ActivePlayer.PlayerChanged += SetInventory;
            inventoryMenu.SetInventory(ActivePlayer.GetInventory());

            leftCanvasScreens.Switched += _ => FloatCameraLeft();
            rightCanvasScreens.Switched += _ => FloatCameraRight();

            // _bus.Subscribe<ScreenActionSignal>(OnScreenActionSignal);
        }

        private void OnDestroy()
        {
            ActivePlayer.PlayerChanged -= SetInventory;
            // _bus.Unsubscribe<ScreenActionSignal>(OnScreenActionSignal);
        }

        private void SetInventory() => inventoryMenu.SetInventory(ActivePlayer.GetInventory());

        // private void OnScreenActionSignal(ScreenActionSignal signal)
        // {
        //     if (signal.ScreenName == ScreenNames.OtherInventory)
        //     {
        //         lootedInventoryMenu.SetInventory(signal.Data as Inventory);
        //         lootedInventoryMenu.ApplyScreenAction(signal.ScreenAction);
        //     }
        // }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                _bus.Fire(new ScreenActionSignal(ScreenNames.Pause, ScreenAction.Switch));
            if (Input.GetKeyDown(KeyCode.C))
                _bus.Fire(new ScreenActionSignal(ScreenNames.Stats, ScreenAction.Switch));
            if (Input.GetKeyDown(KeyCode.B))
                _bus.Fire(new ScreenActionSignal(ScreenNames.PlayerInventory, ScreenAction.Switch));
        }

        private void FloatCameraLeft()
        {
            FloatScreen(leftCanvasScreens);
        }

        private void FloatCameraRight()
        {
            FloatScreen(rightCanvasScreens);
        }

        private void FloatScreen(BaseScreen screen)
        {
            var rectTransform = screen.GetComponent<RectTransform>();
            var cam = Camera.main;
            float offsetPixels = rectTransform.sizeDelta.x * transform.localScale.x *
                                 (screen.IsOpened ? 1 : -1) *
                                 (rectTransform.anchorMin.x < 0.5 ? 1 : -1);
            _currentTransposerTargetValue += offsetPixels / cam.pixelWidth / 2;
            DOTween.To(() => _transposer.m_ScreenX, value => _transposer.m_ScreenX = value,
                _currentTransposerTargetValue, screenAnimationTime);
        }

        private void CalculateCameraOffset()
        {
            float targetValue = _defaultTransposerTargetValue;
            if (leftCanvasScreens.IsOpened) targetValue -= sideScreensCameraOffset;
            if (rightCanvasScreens.IsOpened) targetValue += sideScreensCameraOffset;
            _currentTransposerTargetValue = targetValue;
            DOTween.To(() => _transposer.m_ScreenX, value => _transposer.m_ScreenX = value,
                _currentTransposerTargetValue, screenAnimationTime);
        }
    }
}