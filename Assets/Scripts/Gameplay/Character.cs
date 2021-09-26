using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay.Commands;
using SO;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Utility;
using Utility.Modifiers;

namespace Gameplay {
    [RequireComponent(typeof(NavMeshAgent))]
    public class Character : Mortal {
        public CharacterTypeSO characterType;
        public HitRecoveryConfigSO hitRecoveryConfig;
        public MovementConfigSO movementConfig;
        public AttackConfigSO attackConfig;

        public event Action<Command> StartedExecuting;
        public event Action<Command> StoppedExecuting;

        public ModifiedParameter<bool> areActionsBlocked = new ModifiedParameter<bool>(
            false, (t1, t2) => t1 || t2);

        public ModifiedParameter<float> speed = new ModifiedParameter<float>(
            1f, (t1, t2) => t1 * t2);

        public Command CurrentCommand { get; private set; }
        public NavMeshAgent NavAgent { get; private set; }
        public bool IsStarted { get; private set; }

        protected override void Start() {
            base.Start();
            NavAgent = GetComponent<NavMeshAgent>();
            speed.BaseValue = movementConfig.moveSpeed;
            speed.Changed += newSpeed => NavAgent.speed = newSpeed;
            NavAgent.speed = speed;
            IsStarted = true;
        }

        void FixedUpdate() {
            CurrentCommand?.Update(Time.fixedDeltaTime);
        }

        public override void Hit(float damage, GameObject source) {
            base.Hit(damage, source);
            Debug.Log($"{gameObject.name} got hit from {source.name} by {damage}");
            if (damage >= healthConfig.maxHealth * hitRecoveryConfig.stunDamagePercentThreshold) {
                ForceExecute(new HitRecoveryCommand());
            }
        }

        public void Execute(Command command) {
            if (areActionsBlocked) return;
            ForceExecute(command);
        }

        void ForceExecute(Command command) {
            CurrentCommand?.Stop(false);
            CurrentCommand = command;
            CurrentCommand.Executed += OnCommandExecuted;
            StartedExecuting?.Invoke(CurrentCommand);
            CurrentCommand.Start(this);
        }

        void OnCommandExecuted(bool result) {
            var command = CurrentCommand;
            CurrentCommand = null;
            StoppedExecuting?.Invoke(command);
        }

        protected override void Die() {
            CurrentCommand?.Stop(false);
            StopAllCoroutines();
            NavAgent.ResetPath();
            base.Die();
        }
    }
}