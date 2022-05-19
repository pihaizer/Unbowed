using Sirenix.OdinInspector;
using Unbowed.Utility.Modifiers;
using UnityEngine;
using UnityEngine.AI;

namespace Unbowed.Gameplay.Characters.Modules
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Movement : MonoBehaviour
    {
        public ModifiableParameter<float> speed;

        private float runningSpeedMultiplier = 2f;
        private bool _isRunning;
        private Modifier<float> _runningSpeedModifier;

        public float Speed => speed;
        public NavMeshAgent NavAgent { get; private set; }

        public void Init(float baseSpeed)
        {
            NavAgent = GetComponent<NavMeshAgent>();
            speed = new ModifiableParameter<float>(baseSpeed);
            speed.Changed += newValue => NavAgent.speed = newValue;
            NavAgent.speed = speed;
            _runningSpeedModifier = new Modifier<float>(runningSpeedMultiplier, Operations.Mul);
        }

        public void ToggleRunning() => SetRunning(!_isRunning);

        private void SetRunning(bool value)
        {
            if (_isRunning == value) return;
            _isRunning = value;
            if (_isRunning)
                speed.AddModifier(_runningSpeedModifier);
            else
                speed.RemoveModifier(_runningSpeedModifier);
        }

        public void Stop()
        {
            if (NavAgent.isOnNavMesh) NavAgent.ResetPath();
        }
    }
}