﻿using Unbowed.Gameplay.Characters.Commands;
using Unbowed.SO.Brains;
using Unbowed.Utility;
using UnityEngine;
using UnityEngine.AI;

namespace Unbowed.Gameplay.Characters.AI.Brains {
    public class BasicBrain : Brain {
        readonly BasicBrainConfigSO _config;
        Command _previousCommand;

        public BasicBrain(BasicBrainConfigSO config, Character body, int id) : base(body, id) {
            _config = config;
            body.characterCommandExecutor.StoppedExecuting += OnStoppedExecuting;
        }

        public override void FixedUpdate() {
            base.FixedUpdate();
            if (body.health.isDead || body.areActionsBlocked) return;
            if (body.characterCommandExecutor.MainCommand == null) {
                SelectNewCommand();
            }
        }

        void OnStoppedExecuting(Command characterCommand) {
            _previousCommand = characterCommand;
        }

        void SelectNewCommand() {
            if (!(_previousCommand is AttackCommand && !_previousCommand.Result) &&
                SeesWantedCharacter(out var wantedCharacter)) {
                Attack(wantedCharacter);
            } else if (_previousCommand is IdleCommand) {
                MoveToRandomPoint();
            } else {
                Idle(VectorRandom.Range(_config.idleAfterMoveTimeRange));
            }
        }

        bool SeesWantedCharacter(out Character character) {
            character = null;

            var hits = Physics.OverlapSphere(body.transform.position, _config.playerAggroRange);
            foreach (var hit in hits) {
                var hitCharacter = hit.GetComponentInParent<Character>();
                if (hitCharacter && IsWanted(hitCharacter) && Sees(hitCharacter) && CanGoTo(hitCharacter)) {
                    character = hitCharacter;
                    break;
                }
            }

            return character;
        }

        bool IsWanted(Character character) {
            return character != body && character.CanBeHit() && _config.targetTypes.Contains(character.characterType);
        }

        bool Sees(Character other) {
            var thisHead = body.transform.position + Vector3.up * (body.movement.NavAgent.height + body.movement.NavAgent.baseOffset);
            var otherHead = other.transform.position + Vector3.up * (other.movement.NavAgent.height + other.movement.NavAgent.baseOffset);
            bool sees = !Physics.Raycast(thisHead, otherHead - thisHead,
                Vector3.Distance(thisHead, otherHead), _config.sightLayerMask);
            Debug.DrawLine(thisHead, otherHead, sees ? Color.green : Color.red);
            return sees;
        }

        bool CanGoTo(Character character) {
            var path = new NavMeshPath();
            
            if (NavMesh.CalculatePath(body.transform.position,
                character.transform.position, NavMesh.AllAreas, path)) {
                return path.GetLength() < character.config.distances.maxChaseRange;
            }

            return false;
        }

        void MoveToRandomPoint() {
            for (int i = 0; i < 10; i++) {
                var direction2d = Random.insideUnitCircle.normalized * VectorRandom.Range(_config.newPointsRadiusRange);
                var direction = new Vector3(direction2d.x, 0, direction2d.y);
                var newPoint = body.transform.position + direction;
                NavMesh.SamplePosition(newPoint, out var hit, _config.maxWalkDistance, NavMesh.AllAreas);
                var movePosition = hit.position;
                if (!IsMovePositionValid(movePosition)) continue;
                MoveToPoint(movePosition, _config.maxWalkTime);
                return;
            }

            if (restrictedZone) {
                var point = restrictedZone.GetClosestPoint(body.transform.position);
                NavMesh.SamplePosition(point, out var hit, _config.maxWalkDistance, NavMesh.AllAreas);
                MoveToPoint(hit.position);
            }
        }
    }
}