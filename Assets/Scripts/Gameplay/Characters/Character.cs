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

        public event Action<CharacterCommand> StartedExecuting;
        public event Action<CharacterCommand> StoppedExecuting;

        public ModifiedParameter<bool> areActionsBlocked = new ModifiedParameter<bool>(
            false, (t1, t2) => t1 || t2);

        public ModifiedParameter<float> speed = new ModifiedParameter<float>(
            1f, (t1, t2) => t1 * t2);

        public CharacterCommand CurrentCharacterCommand { get; private set; }
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
            CurrentCharacterCommand?.Update(Time.fixedDeltaTime);
        }

        public override void Hit(float damage, GameObject source) {
            base.Hit(damage, source);
            Debug.Log($"{gameObject.name} got hit from {source.name} by {damage}");
            if (damage >= healthConfig.maxHealth * hitRecoveryConfig.stunDamagePercentThreshold) {
                ForceExecute(new HitRecoveryCommand());
            }
        }

        public void Execute(CharacterCommand characterCommand) {
            if (areActionsBlocked) return;
            ForceExecute(characterCommand);
        }

        void ForceExecute(CharacterCommand characterCommand) {
            CurrentCharacterCommand?.Stop(false);
            CurrentCharacterCommand = characterCommand;
            CurrentCharacterCommand.Executed += OnCommandExecuted;
            StartedExecuting?.Invoke(CurrentCharacterCommand);
            CurrentCharacterCommand.Start(this);
        }

        void OnCommandExecuted(bool result) {
            var command = CurrentCharacterCommand;
            CurrentCharacterCommand = null;
            StoppedExecuting?.Invoke(command);
        }

        protected override void Die() {
            CurrentCharacterCommand?.Stop(false);
            StopAllCoroutines();
            NavAgent.ResetPath();
            base.Die();
        }
    }
}