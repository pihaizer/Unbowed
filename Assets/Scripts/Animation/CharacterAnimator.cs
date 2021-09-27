using Gameplay;
using Gameplay.Commands;
using UnityEngine;
using UnityEngine.AI;

namespace DefaultNamespace {
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
            character.StartedExecuting += CharacterOnStartedExecuting;
            character.StoppedExecuting += CharacterOnStoppedExecuting;
            character.isDead.Changed += (value) => {
                if (value)
                    OnDied();
                else
                    OnRevived();
            };
        }

        void CharacterOnStartedExecuting(CharacterCommand characterCommand) {
            if (characterCommand is AttackCommand attackCommand) {
                attackCommand.isAttacking.Changed += OnAttacking;
            } else if (characterCommand is HitRecoveryCommand) {
                OnHitRecovering(true);
            }
        }

        void CharacterOnStoppedExecuting(CharacterCommand characterCommand) {
            if (characterCommand is AttackCommand attackCommand) {
                attackCommand.isAttacking.Changed -= OnAttacking;
            } else if (characterCommand is HitRecoveryCommand) {
                OnHitRecovering(false);
            }
        }

        void Update() {
            float relativeSpeed = character.NavAgent.hasPath
                ? character.speed.ModifiedValue / character.speed.BaseValue
                : 0;
            animator.SetFloat(RelativeSpeed, relativeSpeed, 0.1f, Time.deltaTime);
        }

        void OnAttacking(bool value) {
            if (value) {
                animator.SetTrigger(Attack);
                float attackSpeed = attackAnimation.length / character.attackConfig.attackTime;
                animator.SetFloat(AttackSpeed, attackSpeed);
            }
        }

        void OnHitRecovering(bool value) {
            if (value) {
                animator.SetTrigger(GotHit);
                float hitRecoverSpeed = gotHitAnimation.length / character.hitRecoveryConfig.hitRecoveryTime;
                animator.SetFloat(HitRecoverySpeed, hitRecoverSpeed);
            }
        }

        void OnDied() {
            animator.SetTrigger(Died);
        }

        void OnRevived() {
            animator.ResetTrigger(GotHit);
            animator.ResetTrigger(Died);
            // animator.SetTrigger(Revived);
        }
    }
}