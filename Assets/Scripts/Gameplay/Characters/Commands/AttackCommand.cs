using System.Collections;
using Unbowed.Gameplay.Characters.Configs.Stats;
using Unbowed.Gameplay.Characters.Stats;
using Unbowed.Utility;
using Unbowed.Utility.Modifiers;
using UnityEngine;
using UnityEngine.AI;

namespace Unbowed.Gameplay.Characters.Commands {
    public class AttackCommand : CharacterCommand {
        public readonly Mutable<bool> isAttacking = new Mutable<bool>();

        public IHittable Target { get; }
        readonly float _maxTime;
        Character _character;
        IEnumerator _coroutine;
        readonly Modifier<bool> _actionsBlock = new Modifier<bool>(true, Operations.Or);
        float _startTime;


        public AttackCommand(IHittable target, float maxTime = float.MaxValue) {
            Target = target;
            _maxTime = maxTime;
        }

        public override void Start(Character character) {
            _character = character;
            _startTime = Time.time;
        }

        public override void Update(float deltaTime) {
            base.Update(deltaTime);

            if (isAttacking) return;

            if (TryAttack(Target)) return;

            if (Time.time > _startTime + _maxTime) {
                Stop(false);
                return;
            }

            GoToTarget(Target);
        }

        public override void Stop(bool result) {
            if (_coroutine != null) _character.StopCoroutine(_coroutine);
            _character.areActionsBlocked.RemoveModifier(_actionsBlock);
            _character.movement.Stop();
            isAttacking.Set(false);
            base.Stop(result);
        }

        void GoToTarget(IHittable target) {
            var path = new NavMeshPath();
            var newMoveTarget = target.GetGameObject().transform.position;

            _character.movement.NavAgent.CalculatePath(newMoveTarget, path);

            if (path.status == NavMeshPathStatus.PathInvalid) {
                Debug.Log("Stopping attack due to invalid path");
                Stop(false);
                return;
            }

            _character.movement.NavAgent.SetPath(path);

            if (_character.movement.NavAgent.hasPath &&
                _character.movement.NavAgent.GetRemainingDistance() > _character.config.distances.maxChaseRange) {
                Debug.Log($"Stopping attack due to remaining distance {_character.movement.NavAgent.GetRemainingDistance()}");
                Stop(false);
            }
        }

        bool TryAttack(IHittable target) {
            if (!IsWithinAttackRange(target)) return false;

            _character.movement.NavAgent.ResetPath();
            _coroutine = AttackCoroutine(target);
            _character.StartCoroutine(_coroutine);
            return true;
        }

        bool IsWithinAttackRange(IHittable target) {
            if (target == null || target.GetGameObject() == null) return false;

            return (target.GetGameObject().transform.position - _character.transform.position).sqrMagnitude <
                   Mathf.Pow(_character.config.distances.attackRadius, 2);
        }

        IEnumerator AttackCoroutine(IHittable target) {
            float attackTime = _character.Stats[StatType.AttackTime];
            var damageRange = new Vector2Int(
                Mathf.FloorToInt(_character.Stats[StatType.MinDamage]),
                Mathf.FloorToInt(_character.Stats[StatType.MaxDamage]));
            _character.areActionsBlocked.AddModifier(_actionsBlock);
            isAttacking.Set(true);
            _character.transform.LookAt(target.GetGameObject().transform);
            yield return new WaitForSeconds(attackTime * _character.config.animationConfig.hitMomentPercent);
            target.Hit(VectorRandom.Range(damageRange), _character);
            yield return new WaitForSeconds(attackTime * (1 - _character.config.animationConfig.hitMomentPercent));
            _character.areActionsBlocked.RemoveModifier(_actionsBlock);
            isAttacking.Set(false);
            Stop(true);
        }

        public override string ToString() => $"Attacking {Target.GetGameObject()}";
    }
}