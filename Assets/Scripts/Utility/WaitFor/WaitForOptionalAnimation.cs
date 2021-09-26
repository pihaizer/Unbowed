using System;
using UnityEngine;

namespace Utility {
    public class WaitForOptionalAnimation : CustomYieldInstruction {
        readonly AnimationClip _clip;
        readonly float _startTime;

        public override bool keepWaiting => _clip != null && (Time.time - _startTime) < _clip.length;

        public WaitForOptionalAnimation(AnimationClip clip) {
            _clip = clip;
            _startTime = Time.time;
        }
    }
}