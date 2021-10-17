using System;

using Sirenix.OdinInspector;

using Unbowed.Gameplay.Characters;
using Unbowed.Gameplay.Characters.Commands;
using Unbowed.Gameplay.Characters.Configs;
using Unbowed.Gameplay.Characters.Modules;
using Unbowed.Gameplay.Characters.Stats;
using Unbowed.SO;
using Unbowed.Utility.Modifiers;

using UnityEngine;

using Item = Unbowed.Gameplay.Characters.Items.Item;

namespace Unbowed.Gameplay.Characters {
    [RequireComponent(typeof(Modules.Health))]
    [RequireComponent(typeof(Modules.Effects))]
    [RequireComponent(typeof(Modules.Movement))]
    [RequireComponent(typeof(Modules.Inventory))]
    [RequireComponent(typeof(Modules.Commands))]
    [RequireComponent(typeof(Modules.Drops))]
    [ExecuteAlways]
    public class Character : SerializedMonoBehaviour, ISelectable, IHittable {
        // configs
        public CharacterTypeSO characterType;
        public CharacterConfig config;

        // modules
        [HideInInspector]
        public Modules.Health health;

        [HideInInspector]
        public Modules.Effects effects;

        [HideInInspector]
        public Modules.Movement movement;

        [HideInInspector]
        public Modules.Inventory inventory;
        
        [HideInInspector]
        public Modules.Commands commands;

        [HideInInspector]
        public Modules.Drops drops;

        // runtime values
        public bool IsStarted { get; private set; }
        
        [ShowInInspector]
        public Stats.Stats Stats { get; private set; }

        [NonSerialized]
        public readonly ModifiableParameter<bool> areActionsBlocked = new ModifiableParameter<bool>();

        void OnEnable() {
            health = GetComponent<Modules.Health>();
            movement = GetComponent<Modules.Movement>();
            inventory = GetComponent<Modules.Inventory>();
            commands = GetComponent<Modules.Commands>();
            drops = GetComponent<Modules.Drops>();
            effects = GetComponent<Modules.Effects>();
        }

        protected void Start() {
            if (!Application.isPlaying) return;

            Stats = new Stats.Stats(config.stats);
            Stats.Update();

            InitHealth();
            InitSpeed();
            InitInventory();
            InitCommandExecutor();
            InitDropsModule();

            IsStarted = true;
        }

        public void Hit(int damage, Character source) {
            health.Hit(damage, source);
            if (damage >= health.Max * CharacterConstants.StunDamageThreshold) {
                commands.ForceExecute(new HitRecoveryCommand());
            }
        }

        public bool TryUseItem(Item item) {
            if (!item.IsUsable) return false;
            item.Config.usableItem.appliedEffect.Build().Apply(this);
            Inventory.RemoveItem(item);
            return true;
        }

        void InitHealth() {
            health.Init(Mathf.FloorToInt(Stats[StatType.Health]));
            health.Died += OnDeath;
            health.Revived += OnRevive;
        }

        void InitSpeed() => movement.Init(Stats[StatType.MoveSpeed]);

        void InitInventory() {
            inventory.AddedItem += InventoryOnAddedItem;
            inventory.RemovedItem += InventoryOnRemovedItem;
            inventory.Init();
        }

        void InventoryOnAddedItem(Item item) {
            if (item.statEffectorsBundle == null) return;
            if (item.IsEquipped) Stats.AddModifier(item.statEffectorsBundle);
        }

        void InventoryOnRemovedItem(Item item) {
            if (item.statEffectorsBundle == null) return;
            if (item.IsEquipped) Stats.RemoveModifier(item.statEffectorsBundle);
        }

        void InitCommandExecutor() => commands.Init(this);

        void InitDropsModule() => drops.Init(config.dropsConfig);

        void OnDeath(DeathData data) {
            commands.StopMain();
            movement.NavAgent.enabled = false;
            StopAllCoroutines();
            movement.Stop();
        }

        void OnRevive() {
            gameObject.SetActive(true);
            movement.NavAgent.enabled = true;
        }

        public bool CanBeHit() => !health.isDead;

        public GameObject GetGameObject() => gameObject;

        public string GetName() => gameObject.name;

        public bool CanBeSelected() => !health.isDead;

        public bool HasTargetUI() => true;
    }
}