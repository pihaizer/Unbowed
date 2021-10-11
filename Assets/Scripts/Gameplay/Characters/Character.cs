using System;

using Sirenix.OdinInspector;

using Unbowed.Gameplay.Characters.Commands;
using Unbowed.Gameplay.Characters.Configs;
using Unbowed.Gameplay.Characters.Configs.Stats;
using Unbowed.Gameplay.Characters.Modules;
using Unbowed.Gameplay.Items;
using Unbowed.SO;
using Unbowed.Utility.Modifiers;

using UnityEngine;

namespace Unbowed.Gameplay.Characters {
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(CharacterMovement))]
    [RequireComponent(typeof(Inventory))]
    [RequireComponent(typeof(CharacterCommandExecutor))]
    [RequireComponent(typeof(DropsModule))]
    [ExecuteAlways]
    public class Character : SerializedMonoBehaviour, ISelectable, IHittable {
        // configs
        public CharacterTypeSO characterType;
        public CharacterConfig config;

        // modules
        [HideInInspector]
        public Health health;

        [HideInInspector]
        public CharacterMovement movement;

        [HideInInspector]
        public Inventory inventory;
        [HideInInspector]
        public CharacterCommandExecutor characterCommandExecutor;

        [HideInInspector]
        public DropsModule dropsModule;

        // runtime values
        public bool IsStarted { get; private set; }
        
        [ShowInInspector]
        public Stats Stats { get; private set; }

        [NonSerialized]
        public readonly ModifiableParameter<bool> areActionsBlocked = new ModifiableParameter<bool>();

        void OnEnable() {
            health = GetComponent<Health>();
            movement = GetComponent<CharacterMovement>();
            inventory = GetComponent<Inventory>();
            characterCommandExecutor = GetComponent<CharacterCommandExecutor>();
            dropsModule = GetComponent<DropsModule>();
        }

        protected void Start() {
            if (!Application.isPlaying) return;

            Stats = new Stats(config.stats);
            Stats.Update();

            InitHealth();
            InitSpeed();
            InitInventory();
            InitCommandExecutor();
            InitDropsModule();

            IsStarted = true;
        }

        void InitHealth() {
            health.Init(Mathf.FloorToInt(Stats["health"]));
            health.Died += OnDeath;
            health.Revived += OnRevive;
        }

        void InitSpeed() => movement.Init(Stats["MoveSpeed"]);

        void InitInventory() {
            inventory.AddedItem += InventoryOnAddedItem;
            inventory.RemovedItem += InventoryOnRemovedItem;
            inventory.Init();
        }

        void InventoryOnAddedItem(Item item) {
            if (item.statModifiersContainer == null) return;
            if (item.IsEquipped) Stats.AddModifier(item.statModifiersContainer);
        }

        void InventoryOnRemovedItem(Item item) {
            if (item.statModifiersContainer == null) return;
            if (item.IsEquipped) Stats.RemoveModifier(item.statModifiersContainer);
        }

        void InitCommandExecutor() => characterCommandExecutor.Init(this);

        void InitDropsModule() => dropsModule.Init(config.dropsConfig);

        void OnDeath(DeathData data) {
            characterCommandExecutor.StopMain();
            movement.NavAgent.enabled = false;
            StopAllCoroutines();
            movement.Stop();
        }

        protected void OnRevive() {
            gameObject.SetActive(true);
            movement.NavAgent.enabled = true;
        }

        public void Hit(int damage, Character source) {
            health.Hit(damage, source);
            if (damage >= health.Max * CharacterConstants.StunDamageThreshold) {
                characterCommandExecutor.ForceExecute(new HitRecoveryCommand());
            }
        }

        public bool CanBeHit() => !health.isDead;

        public GameObject GetGameObject() => gameObject;

        public string GetName() => gameObject.name;

        public bool CanBeSelected() => !health.isDead;

        public bool HasTargetUI() => true;
    }
}