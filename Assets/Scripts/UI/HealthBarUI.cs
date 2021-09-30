using DG.Tweening;
using Unbowed.Gameplay;
using Unbowed.SO.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Unbowed.UI {
    public class HealthBarUI : MonoBehaviour {
        [SerializeField] HealthChangedEventSO healthChangedEventSO;
        [SerializeField] Image fillImage;
        [SerializeField] float animationTime;
        [SerializeField] Ease animationEase;

        void Awake() {
            if (healthChangedEventSO) healthChangedEventSO.AddListener(OnHealthChanged);
        }

        public void OnHealthChanged(HealthChangeData data) {
            fillImage.DOFillAmount((float) data.target.Current / data.target.Max, animationTime)
                .SetEase(animationEase);
        }

        public void SetHealthPercent(float value) => SetFill(value);

        void SetFill(float value) => fillImage.fillAmount = value;
    }
}