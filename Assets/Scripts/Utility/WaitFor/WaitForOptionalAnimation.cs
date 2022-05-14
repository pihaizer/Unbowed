using UnityEngine;

namespace Unbowed.Utility.WaitFor {
    public class WaitForOptionalAnimation : CustomYieldInstruction {
        private readonly AnimationClip _clip;
        private readonly float _startTime;

        public override bool keepWaiting => _clip != null && (Time.time - _startTime) < _clip.length;

        public WaitForOptionalAnimation(AnimationClip clip) {
            _clip = clip;
            _startTime = Time.time;
        }
    }
}