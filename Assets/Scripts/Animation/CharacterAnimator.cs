using Unbowed.Gameplay;
using Unbowed.Gameplay.Characters;
using Unbowed.Gameplay.Characters.Commands;
using Unbowed.Gameplay.Characters.Configs.Stats;
using Unbowed.Gameplay.Characters.Stats;

using UnityEngine;

namespace Unbowed.Animation {
    public class CharacterAnimator : MonoBehaviour {
        [SerializeField] private Character character;
        [SerializeField] private Animator animator;
        [SerializeField] private AnimationClip attackAnimation;
        [SerializeField] private AnimationClip gotHitAnimation;
        [SerializeField] private GameObject ragdollGameobject;

        private static readonly int RelativeSpeed = Animator.StringToHash("relativeSpeed");
        private static readonly int Attack = Animator.StringToHash("attack");
        private static readonly int GotHit = Animator.StringToHash("gotHit");
        private static readonly int Died = Animator.StringToHash("died");
        private static readonly int Revived = Animator.StringToHash("revived");
        private static readonly int AttackSpeed = Animator.StringToHash("attackSpeed");
        private static readonly int HitRecoverySpeed = Animator.StringToHash("hitRecoverySpeed");

        private void Start() {
            character.commands.StartedExecuting += CharacterOnStartedExecuting;
            character.commands.StoppedExecuting += CharacterOnStoppedExecuting;
            character.health.Died += OnDied;
            character.health.Revived += OnRevived;

            SetRagdollActive(false);
        }

        private void Update() {
            float relativeSpeed = character.movement.NavAgent.hasPath
                ? character.movement.speed.ModifiedValue / character.movement.speed.BaseValue
                : 0;
            animator.SetFloat(RelativeSpeed, relativeSpeed, 0.1f, Time.deltaTime);
        }

        private void CharacterOnStartedExecuting(Command command) {
            if (command is AttackCommand attackCommand) {
                attackCommand.isAttacking.Changed += OnAttacking;
            } else if (command is HitRecoveryCommand) {
                OnHitRecovering(true);
            }
        }

        private void CharacterOnStoppedExecuting(Command command) {
            if (command is AttackCommand attackCommand) {
                attackCommand.isAttacking.Changed -= OnAttacking;
            } else if (command is HitRecoveryCommand) {
                OnHitRecovering(false);
            }
        }

        private void OnAttacking(bool value) {
            if (value) {
                animator.SetTrigger(Attack);
                float attackSpeed = attackAnimation.length / character.Stats[StatType.AttackTime];
                animator.SetFloat(AttackSpeed, attackSpeed);
            }
        }

        private void OnHitRecovering(bool value) {
            if (value) {
                animator.SetTrigger(GotHit);
                float hitRecoverSpeed = gotHitAnimation.length / character.Stats[StatType.HitRecoveryTime];
                animator.SetFloat(HitRecoverySpeed, hitRecoverSpeed);
            }
        }

        private void OnDied(DeathData data)
        {
            animator.enabled = false;
            SetRagdollActive(true);
        }

        private void SetRagdollActive(bool value)
        {
            foreach (Rigidbody rigidbody in ragdollGameobject.GetComponentsInChildren<Rigidbody>())
            {
                rigidbody.detectCollisions = value;
                rigidbody.isKinematic = !value;
            }
        }

        private void OnRevived() {
            SetRagdollActive(false);
            animator.enabled = true;
            animator.ResetTrigger(GotHit);
            animator.ResetTrigger(Died);
            animator.SetTrigger(Revived);
        }
    }
}