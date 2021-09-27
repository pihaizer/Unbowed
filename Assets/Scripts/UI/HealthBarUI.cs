using DG.Tweening;
using Gameplay;
using SO.Events;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class HealthBarUI : MonoBehaviour {
        [SerializeField] HealthChangedEventSO healthChangedEventSO;
        [SerializeField] Image fillImage;
        [SerializeField] float animationTime;
        [SerializeField] Ease animationEase;

        void Awake() {
            if(healthChangedEventSO)
                healthChangedEventSO.AddListener(OnHealthChanged);
        }

        public void SetHealthPercent(float value) => SetFill(value);

        public void OnHealthChanged(HealthChangeData data) {
            fillImage.DOFillAmount(data.target.GetHealthPercent(), animationTime).SetEase(animationEase);
        }

        void SetFill(float value) => fillImage.fillAmount = value;
    }
}