using System;
using Sirenix.OdinInspector;
using Unbowed.Gameplay.Characters.Commands;
using Unbowed.Gameplay.Characters.Configs;
using Unbowed.Gameplay.Characters.Configs.Stats;
using Unbowed.SO;
using Unbowed.Utility.Modifiers;
using UnityEngine;
using UnityEngine.AI;

namespace Unbowed.Gameplay.Characters {
    [RequireComponent(typeof(NavMeshAgent))]
    public class Character : MonoBehaviour, ISelectable, IHittable {
        public CharacterTypeSO characterType;
        public CharacterConfig config;
        public CharacterRuntimeStats stats;

        public HealthModule Health { get; } = new HealthModule();
        public MovementModule Movement { get; } = new MovementModule();

        public event Action<CharacterCommand> StartedExecuting;
        public event Action<CharacterCommand> StoppedExecuting;
        
        public readonly ModifiableParameter<bool> areActionsBlocked = new ModifiableParameter<bool>();

        public ModifiableParameter<float> speed;


        public CharacterCommand CurrentCharacterCommand { get; private set; }
        public bool IsStarted { get; private set; }

        [ShowInInspector] string CurrentCommandString => CurrentCharacterCommand?.ToString();

        protected virtual void Start() {
            stats = new CharacterRuntimeStats(config.baseStats);
            
            InitHealth();
            InitSpeed();
            IsStarted = true;
        }

        void InitHealth() {
            Health.Init(Mathf.FloorToInt(stats[StatType.Endurance]) * 5);
            Health.isDead.Changed += (becameDead) => {
                if (becameDead)
                    OnDeath();
                else
                    OnRevive();
            };
        }

        void InitSpeed() {
            Movement.Init(GetComponent<NavMeshAgent>(), stats[StatType.MoveSpeed]);
            speed = new ModifiableParameter<float>(stats[StatType.MoveSpeed]);
        }

        void FixedUpdate() {
            CurrentCharacterCommand?.Update(Time.fixedDeltaTime);
        }

        public void Hit(int damage, GameObject source) {
            Health.Hit(damage, source);
            if (damage >= Health.Max * CharacterConstants.StunDamageThreshold) {
                ForceExecute(new HitRecoveryCommand());
            }
        }

        public bool CanBeHit() => !Health.isDead;

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

        protected virtual void OnDeath() {
            CurrentCharacterCommand?.Stop(false);
            StopAllCoroutines();
            Movement.Stop();
        }

        protected virtual void OnRevive() {
            gameObject.SetActive(true);
        }

        public GameObject GetGameObject() => gameObject;

        public string GetName() => gameObject.name;

        public bool CanBeSelected() => !Health.isDead;
    }
}