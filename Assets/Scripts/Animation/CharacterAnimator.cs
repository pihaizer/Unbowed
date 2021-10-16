using Unbowed.Gameplay;
using Unbowed.Gameplay.Characters;
using Unbowed.Gameplay.Characters.Commands;
using Unbowed.Gameplay.Characters.Configs.Stats;
using Unbowed.Gameplay.Characters.Stats;

using UnityEngine;

namespace Unbowed.Animation {
    public class CharacterAnimator : MonoBehaviour {
        [SerializeField] Character character;
        [SerializeField] Animator animator;
        [SerializeField] AnimationClip attackAnimation;
        [SerializeField] AnimationClip gotHitAnimation;

        static readonly int RelativeSpeed = Animator.StringToHash("relativeSpeed");
        static readonly int Attack = Animator.StringToHash("attack");
        static readonly int GotHit = Animator.StringToHash("gotHit");
        static readonly int Died = Animator.StringToHash("died");
        static readonly int Revived = Animator.StringToHash("revived");
        static readonly int AttackSpeed = Animator.StringToHash("attackSpeed");
        static readonly int HitRecoverySpeed = Animator.StringToHash("hitRecoverySpeed");

        void Start() {
            character.characterCommandExecutor.StartedExecuting += CharacterOnStartedExecuting;
            character.characterCommandExecutor.StoppedExecuting += CharacterOnStoppedExecuting;
            character.health.Died += OnDied;
            character.health.Revived += OnRevived;
        }

        void Update() {
            float relativeSpeed = character.movement.NavAgent.hasPath
                ? character.movement.speed.ModifiedValue / character.movement.speed.BaseValue
                : 0;
            animator.SetFloat(RelativeSpeed, relativeSpeed, 0.1f, Time.deltaTime);
        }

        void CharacterOnStartedExecuting(Command command) {
            if (command is AttackCommand attackCommand) {
                attackCommand.isAttacking.Changed += OnAttacking;
            } else if (command is HitRecoveryCommand) {
                OnHitRecovering(true);
            }
        }

        void CharacterOnStoppedExecuting(Command command) {
            if (command is AttackCommand attackCommand) {
                attackCommand.isAttacking.Changed -= OnAttacking;
            } else if (command is HitRecoveryCommand) {
                OnHitRecovering(false);
            }
        }

        void OnAttacking(bool value) {
            if (value) {
                animator.SetTrigger(Attack);
                float attackSpeed = attackAnimation.length / character.Stats[StatType.AttackTime];
                animator.SetFloat(AttackSpeed, attackSpeed);
            }
        }

        void OnHitRecovering(bool value) {
            if (value) {
                animator.SetTrigger(GotHit);
                float hitRecoverSpeed = gotHitAnimation.length / character.Stats[StatType.HitRecoveryTime];
                animator.SetFloat(HitRecoverySpeed, hitRecoverSpeed);
            }
        }

        void OnDied(DeathData data) {
            animator.SetTrigger(Died);
        }

        void OnRevived() {
            animator.ResetTrigger(GotHit);
            animator.ResetTrigger(Died);
            animator.SetTrigger(Revived);
        }
    }
}