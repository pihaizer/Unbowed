﻿using Unbowed.Gameplay.Characters.Commands;
using UnityEngine;

namespace Unbowed.Gameplay.Characters.AI.Brains {
    public abstract class Brain {
        public int ID { get; }

        protected readonly Character body;
        protected ColliderZone restrictedZone = null;

        protected Brain(Character body, int id) {
            this.body = body;
            ID = id;
        }

        public virtual void FixedUpdate() { }
        public virtual void Update() { }
        public virtual void OnDestroy() {}

        public void SetRestrictedZone(ColliderZone zone) => restrictedZone = zone;

        protected void MoveToPoint(Vector3 point, float maxTime = float.MaxValue) {
            body.commands.Execute(new MoveCommand(point, maxTime));
        }

        protected void Attack(IHittable hittable, float maxTime = float.MaxValue) =>
            body.commands.Execute(new AttackCommand(hittable, maxTime));

        protected void Idle(float time) =>
            body.commands.Execute(new IdleCommand(time));

        protected bool IsMovePositionValid(Vector3 position) =>
            restrictedZone == null || restrictedZone.Contains(position);
    }
}