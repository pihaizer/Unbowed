using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Unbowed.Gameplay.Characters.Commands;
using Unbowed.Gameplay.Characters.Configs;
using Unbowed.Gameplay.Characters.Configs.Stats;
using Unbowed.Gameplay.Characters.Items;
using Unbowed.Gameplay.Characters.Modules;
using Unbowed.SO;
using Unbowed.Utility.Modifiers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Unbowed.Gameplay.Characters {
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(CharacterMovement))]
    [RequireComponent(typeof(Inventory))]
    [RequireComponent(typeof(CharacterCommandExecutor))]
    [ExecuteAlways]
    public class Character : SerializedMonoBehaviour, ISelectable, IHittable {
        // configs
        public CharacterTypeSO characterType;
        public CharacterConfig config;
        public CharacterRuntimeStats stats;
        
        //DEBUG
        public ItemConfig DEBUG_ITEM_CONFIG;
        public ItemConfig DEBUG_ITEM_CONFIG_2;

        // modules
        [HideInInspector]
        public Health health;

        [HideInInspector]
        public CharacterMovement characterMovement;

        [HideInInspector]
        public Inventory inventory;

        [HideInInspector]
        public CharacterCommandExecutor characterCommandExecutor;

        // runtime values
        public bool IsStarted { get; private set; }

        [NonSerialized]
        public readonly ModifiableParameter<bool> areActionsBlocked = new ModifiableParameter<bool>();

        void OnEnable() {
            health = GetComponent<Health>();
            characterMovement = GetComponent<CharacterMovement>();
            inventory = GetComponent<Inventory>();
            characterCommandExecutor = GetComponent<CharacterCommandExecutor>();
        }

        protected virtual void Start() {
            if (!Application.isPlaying) return;
            stats = new CharacterRuntimeStats(config.baseStats);

            InitHealth();
            InitSpeed();
            InitInventory();
            InitCommandExecutor();
            IsStarted = true;
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
            characterMovement.Init(stats[StatType.MoveSpeed]);
        }

        void InitInventory() {
            inventory.Init(16);
            var item1 = new Item(DEBUG_ITEM_CONFIG, new ItemLocation());
            inventory.TryAddItemToInventory(item1);
            inventory.TryAddItemToInventory(item1);
            var item2 = new Item(DEBUG_ITEM_CONFIG, new ItemLocation());
            inventory.TryAddItemToInventory(item2);
            var item3 = new Item(DEBUG_ITEM_CONFIG_2, new ItemLocation());
            inventory.TryAddItemToInventory(item3);
            inventory.TryAddItemToInventory(item3);
        }

        void InitCommandExecutor() {
            characterCommandExecutor.Init(this);
        }

        protected virtual void OnDeath() {
            characterCommandExecutor.StopMain();
            StopAllCoroutines();
            characterMovement.Stop();
        }

        protected virtual void OnRevive() {
            gameObject.SetActive(true);
        }

        public void Hit(int damage, GameObject source) {
            health.Hit(damage, source);
            if (damage >= health.Max * CharacterConstants.StunDamageThreshold) {
                characterCommandExecutor.ForceExecute(new HitRecoveryCommand());
            }
        }

        public bool CanBeHit() => !health.isDead;

        public GameObject GetGameObject() => gameObject;

        public string GetName() => gameObject.name;

        public bool CanBeSelected() => !health.isDead;
    }
}