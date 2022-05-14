using UnityEngine;

namespace Unbowed.Utility.WaitFor {
    public class WaitForSecondsRange : CustomYieldInstruction {
        private readonly float _startTime;
        private readonly float _waitTime;

        public override bool keepWaiting => (Time.time - _startTime) < _waitTime;

        public WaitForSecondsRange(Vector2 timeRange) {
            _waitTime = Random.Range(timeRange.x, timeRange.y);
            _startTime = Time.time;
        }
    }
}