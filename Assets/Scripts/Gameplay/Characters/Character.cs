﻿using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Unbowed.Gameplay.Characters.Commands;
using Unbowed.Gameplay.Characters.Configs;
using Unbowed.Gameplay.Characters.Configs.Stats;
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
    [RequireComponent(typeof(DropsModule))]
    [ExecuteAlways]
    public class Character : SerializedMonoBehaviour, ISelectable, IHittable {
        // configs
        public CharacterTypeSO characterType;
        public CharacterConfig config;
        public CharacterRuntimeStats stats;

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

        [NonSerialized]
        public readonly ModifiableParameter<bool> areActionsBlocked = new ModifiableParameter<bool>();

        void OnEnable() {
            health = GetComponent<Health>();
            movement = GetComponent<CharacterMovement>();
            inventory = GetComponent<Inventory>();
            characterCommandExecutor = GetComponent<CharacterCommandExecutor>();
            dropsModule = GetComponent<DropsModule>();
        }

        protected virtual void Start() {
            if (!Application.isPlaying) return;
            stats = new CharacterRuntimeStats(config.baseStats);

            InitHealth();
            InitSpeed();
            InitInventory();
            InitCommandExecutor();
            InitDropsModule();
            IsStarted = true;
        }

        void InitHealth() {
            health.Init(Mathf.FloorToInt(stats[StatType.Endurance]) * 5);
            health.Died += OnDeath;
            health.Revived += OnRevive;
        }

        void InitSpeed() => movement.Init(stats[StatType.MoveSpeed]);

        void InitInventory() => inventory.Init();

        void InitCommandExecutor() => characterCommandExecutor.Init(this);

        void InitDropsModule() => dropsModule.Init(config.dropsConfig);

        void OnDeath() {
            characterCommandExecutor.StopMain();
            movement.NavAgent.enabled = false;
            StopAllCoroutines();
            movement.Stop();
        }

        protected virtual void OnRevive() {
            gameObject.SetActive(true);
            movement.NavAgent.enabled = true;
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

        public bool HasTargetUI() => true;
    }
}