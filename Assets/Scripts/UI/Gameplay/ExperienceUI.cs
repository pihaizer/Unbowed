using System;
using TMPro;
using Unbowed.Gameplay.Characters;
using Unbowed.Gameplay.Characters.Configs;
using Unbowed.UI.Gameplay.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Unbowed.UI.Gameplay
{
    public class ExperienceUI : MonoBehaviour
    {
        [SerializeField] private Slider _experienceSlider;
        [SerializeField] private TMP_Text _levelText;

        [Inject] private PlayerController _playerController;
        [Inject] private SignalBus _bus;

        private Character _player;

        private void Start()
        {
            _bus.Subscribe<PlayerChangedSignal>(OnPlayerChanged);
            if (!_playerController.Exists) return;
            OnPlayerChanged(new PlayerChangedSignal(_playerController.Get()));
        }

        private void OnPlayerChanged(PlayerChangedSignal data)
        {
            if (_player != null)
            {
                _player.Experience.Experience.Changed -= OnExperienceChanged;
                _player.Experience.Level.Changed -= OnLevelChanged;
            }

            _player = data.Player;
            
            _player.Experience.Experience.Changed += OnExperienceChanged;
            _player.Experience.Level.Changed += OnLevelChanged;
            
            OnExperienceChanged(_player.Experience.Experience);
            OnLevelChanged(_player.Experience.Level);
        }

        private void OnExperienceChanged(long experience)
        {
            _experienceSlider.value = (float)experience /
                                      LevelingConfig.Instance.GetExperienceToLevelUp(_player.Experience.Level + 1);
        }

        private void OnLevelChanged(int level)
        {
            _levelText.text = level.ToString();
        }
    }
}