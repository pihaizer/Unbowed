using DG.Tweening;
using Unbowed.Gameplay;
using UnityEngine;
using UnityEngine.UI;

namespace Unbowed.UI {
    public class HealthBarUI : MonoBehaviour {
        [SerializeField] private Image fillImage;
        [SerializeField] private float animationTime;
        [SerializeField] private Ease animationEase;

        public void OnHealthChanged(HealthChangeData data) {
            float percent = (float) data.target.Current / data.target.Max;
            fillImage.DOFillAmount(percent, animationTime).SetEase(animationEase);
        }

        public void SetHealthPercent(float value) => SetFill(value);

        private void SetFill(float value) => fillImage.fillAmount = value;
    }
}