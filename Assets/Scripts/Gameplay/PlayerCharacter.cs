using UnityEngine;
using Utility.Modifiers;

namespace Gameplay {
    public class PlayerCharacter : Character {
        [SerializeField] float runningSpeedMultiplier = 2f;
        
        bool _isRunning = false;
        readonly Modifier<float> _runningSpeedModifier = new Modifier<float>(1f);

        protected override void Start() {
            base.Start();
            _runningSpeedModifier.Value = runningSpeedMultiplier;
        }

        public void ToggleRunning() => SetRunning(!_isRunning);


        public void SetRunning(bool value) {
            if (_isRunning == value) return;
            _isRunning = value;
            if (_isRunning)
                speed += _runningSpeedModifier;
            else
                speed -= _runningSpeedModifier;
        }
    }
}