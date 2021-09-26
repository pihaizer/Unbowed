using System;
using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;
using Utility;
using Utility.Modifiers;

namespace Gameplay.Commands {
    public class AttackCommand : Command {
        public readonly Mutable<bool> isAttacking = new Mutable<bool>();

        public IHittable Target { get; }
        readonly float _maxTime;
        Character _character;
        IEnumerator _coroutine;
        readonly Modifier<bool> _actionsBlock = new Modifier<bool>(true);
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
            _character.areActionsBlocked -= _actionsBlock;
            _character.NavAgent.ResetPath();
            isAttacking.Set(false);
            base.Stop(result);
        }

        void GoToTarget(IHittable target) {
            var path = new NavMeshPath();
            var newMoveTarget = target.GetGameObject().transform.position;

            _character.NavAgent.CalculatePath(newMoveTarget, path);
            
            if (path.status == NavMeshPathStatus.PathInvalid) {
                Debug.Log("Stopping attack due to invalid path");
                Stop(false);
                return;
            }

            _character.NavAgent.SetPath(path);

            if (_character.NavAgent.hasPath && _character.NavAgent.GetRemainingDistance() > _character.attackConfig.maxChaseRange) {
                Debug.Log($"Stopping attack due to remaining distance {_character.NavAgent.GetRemainingDistance()}");
                Stop(false);
            }
        }

        bool TryAttack(IHittable target) {
            if (!IsWithinAttackRange(target)) return false;

            _character.NavAgent.ResetPath();
            _coroutine = AttackCoroutine(target);
            _character.StartCoroutine(_coroutine);
            return true;
        }

        bool IsWithinAttackRange(IHittable target) {
            if (target == null || target.GetGameObject() == null) return false;

            return (target.GetGameObject().transform.position - _character.transform.position).sqrMagnitude <
                   Mathf.Pow(_character.attackConfig.attackRadius, 2);
        }

        IEnumerator AttackCoroutine(IHittable target) {
            var attackConfig = _character.attackConfig;
            _character.areActionsBlocked += _actionsBlock;
            isAttacking.Set(true);
            _character.transform.LookAt(target.GetGameObject().transform);
            yield return new WaitForSeconds(attackConfig.attackTime * attackConfig.hitTimeMomentPercent);
            target.Hit(attackConfig.attackDamage, _character.gameObject);
            yield return new WaitForSeconds(attackConfig.attackTime * (1 - attackConfig.hitTimeMomentPercent));
            _character.areActionsBlocked -= _actionsBlock;
            isAttacking.Set(false);
            Stop(true);
        }

        public override string ToString() => $"Attacking {Target.GetGameObject()}";
    }
}