using Unbowed.Utility.Modifiers;
using UnityEngine.AI;

namespace Unbowed.Gameplay {
    public class MovementModule {
        public ModifiableParameter<float> speed;
        
        float runningSpeedMultiplier = 2f;
        bool _isRunning;
        Modifier<float> _runningSpeedModifier;
        
        public NavMeshAgent NavAgent { get; private set; }
        

        public void Init(NavMeshAgent agent, float baseSpeed) {
            NavAgent = agent;
            speed = new ModifiableParameter<float>(baseSpeed);
            speed.Changed += newSpeed => NavAgent.speed = newSpeed;
            NavAgent.speed = speed;
            _runningSpeedModifier = new Modifier<float>(runningSpeedMultiplier, Operations.Mul);
        }

        public void ToggleRunning() => SetRunning(!_isRunning);

        void SetRunning(bool value) {
            if (_isRunning == value) return;
            _isRunning = value;
            if (_isRunning)
                speed.AddModifier(_runningSpeedModifier);
            else
                speed.RemoveModifier(_runningSpeedModifier);
        }

        public void Stop() => NavAgent.ResetPath();
    }
}