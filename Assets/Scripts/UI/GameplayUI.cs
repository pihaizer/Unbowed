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

        static Gameplay.Characters.Modules.Inventory PlayerInventory => GlobalContext.Instance.playerCharacter.inventory;

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
            inventoryMenu.ItemClicked += OnItemClicked;
            leftMenus.IsOpened.Changed += (value) => FloatCameraLeft();
            rightMenus.IsOpened.Changed += (value) => FloatCameraRight();
        }
        
        void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) pauseScreen.ToggleActive();
            if (Input.GetKeyDown(KeyCode.C)) characterMenu.ToggleOpened();
            if (Input.GetKeyDown(KeyCode.B)) inventoryMenu.ToggleOpened();
        }

        void OnItemClicked(ItemUI itemUI, PointerEventData data) {
            StartCoroutine(DragItemCoroutine(itemUI));
        }

        IEnumerator DragItemCoroutine(ItemUI itemUI) {
            var item = itemUI.Item;
            var dragItemUI = Instantiate(itemUI, transform);
            Destroy(dragItemUI.GetComponent<GraphicRaycaster>());
            var dragRect = dragItemUI.GetComponent<RectTransform>();
            dragRect.anchorMin = dragRect.anchorMax = Vector2.zero;
            dragRect.pivot = Vector2.one / 2;
            PlayerInventory.RemoveItem(item);
            var raycastResults = new List<RaycastResult>();
            var pointerEventData = new PointerEventData(EventSystem.current);

            var prevCell = new Vector2Int(int.MaxValue,int.MaxValue);
            BagsUI bagsUI = null;

            while(true) {
                if (Input.GetMouseButtonDown(0)) {
                    if (bagsUI != null) {
                        Debug.Log(bagsUI);
                        if (bagsUI.Inventory.TryMoveItemToLocation(item, 
                            new ItemLocation(bagsUI.Inventory, prevCell), out var removedItem)) {
                            if (removedItem != null) {
                                item = removedItem;
                                dragItemUI.SetItem(item);
                            } else {
                                Destroy(dragItemUI.gameObject);
                                break;
                            }
                        }
                    } else {
                        if (!EventSystem.current.IsPointerOverGameObject()) {
                            Destroy(dragItemUI.gameObject);
                            break;
                        }
                    }
                }
                dragRect.anchoredPosition = Input.mousePosition / transform.localScale.x;
                
                pointerEventData.position = Input.mousePosition;    
                EventSystem.current.RaycastAll(pointerEventData, raycastResults);
                
                foreach (var raycastResult in raycastResults) {
                    if (!raycastResult.gameObject.TryGetComponent(out bagsUI)) continue;
                    var bagsRect = bagsUI.GetComponent<RectTransform>();
                    
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        bagsRect, Input.mousePosition,
                        null, out var positionInRect);
                    positionInRect += new Vector2(bagsRect.sizeDelta.x * bagsRect.pivot.x,
                        bagsRect.sizeDelta.y * bagsRect.pivot.y);
                    positionInRect.y = bagsRect.sizeDelta.y - positionInRect.y;
                    positionInRect -= dragRect.rect.size / 2;
                    positionInRect.x *= bagsUI.Size.x / bagsRect.sizeDelta.x;
                    positionInRect.y *= bagsUI.Size.y / bagsRect.sizeDelta.y;
                    var cell = new Vector2Int(Mathf.RoundToInt(positionInRect.x),
                        Mathf.RoundToInt(positionInRect.y));
                    bagsUI.SetAreaHovered(prevCell, item.config.size, false);
                    bagsUI.SetAreaHovered(cell, item.config.size, true);
                    prevCell = cell;
                    break;
                }
                yield return null;
            }
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