using System;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Unbowed.Gameplay.Characters.Commands;
using Unbowed.Gameplay.Characters.Configs;
using Unbowed.Gameplay.Characters.Configs.Stats;
using Unbowed.Gameplay.Characters.Modules;
using Unbowed.SO;
using Unbowed.Utility.Modifiers;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Unbowed.Gameplay.Characters {
    [RequireComponent(typeof(NavMeshAgent))]
    public class Character : SerializedMonoBehaviour, ISelectable, IHittable {
        // configs
        public CharacterTypeSO characterType;
        public CharacterConfig config;
        public CharacterRuntimeStats stats;

        // events
        public event Action<CharacterCommand> StartedExecuting;
        public event Action<CharacterCommand> StoppedExecuting;

        // modules
        [NonSerialized, ShowInInspector]
        public readonly HealthModule health = new HealthModule();

        [NonSerialized, ShowInInspector]
        public readonly MovementModule movement = new MovementModule();

        [NonSerialized, ShowInInspector]
        public readonly InventoryModule inventory = new InventoryModule();

        // runtime values
        public bool IsStarted { get; private set; }
        public CharacterCommand CurrentCharacterCommand { get; private set; }
        [ShowInInspector] string CurrentCommandString => CurrentCharacterCommand?.ToString();

        [NonSerialized, ShowInInspector]
        public readonly ModifiableParameter<bool> areActionsBlocked = new ModifiableParameter<bool>();

        protected virtual void Start() {
            stats = new CharacterRuntimeStats(config.baseStats);

            InitHealth();
            InitSpeed();
            InitInventory();
            IsStarted = true;
        }

        void FixedUpdate() {
            CurrentCharacterCommand?.Update(Time.fixedDeltaTime);
        }

        void InitHealth() {
            health.Init(Mathf.FloorToInt(stats[StatType.Endurance]) * 5);
            health.isDead.Changed += (becameDead) => {
                if (becameDead)
                    OnDeath();
                else
                    OnRevive();
            };
        }

        void InitSpeed() {
            movement.Init(GetComponent<NavMeshAgent>(), stats[StatType.MoveSpeed]);
        }

        void InitInventory() {
            inventory.Init(16);
        }

        public void Hit(int damage, GameObject source) {
            health.Hit(damage, source);
            if (damage >= health.Max * CharacterConstants.StunDamageThreshold) {
                ForceExecute(new HitRecoveryCommand());
            }
        }

        public bool CanBeHit() => !health.isDead;

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
            movement.Stop();
        }

        protected virtual void OnRevive() {
            gameObject.SetActive(true);
        }

        public GameObject GetGameObject() => gameObject;

        public string GetName() => gameObject.name;

        public bool CanBeSelected() => !health.isDead;
    }
}