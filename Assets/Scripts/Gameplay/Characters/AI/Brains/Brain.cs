using Gameplay.Commands;
using UnityEngine;

namespace Gameplay.AI.Brains {
    public abstract class Brain {
        public int ID { get; }

        protected readonly Character body;
        protected ColliderZone restrictedZone = null;

        protected Brain(Character body, int id) {
            this.body = body;
            ID = id;
        }

        public virtual void Update(float deltaTime) { }

        public void SetRestrictedZone(ColliderZone zone) => restrictedZone = zone;

        protected void MoveToPoint(Vector3 point, float maxTime = float.MaxValue) {
            body.Execute(new MoveCommand(point, maxTime));
        }

        protected void Attack(IHittable hittable, float maxTime = float.MaxValue) =>
            body.Execute(new AttackCommand(hittable, maxTime));

        protected void Idle(float time) =>
            body.Execute(new IdleCommand(time));

        protected bool IsMovePositionValid(Vector3 position) =>
            restrictedZone == null || restrictedZone.Contains(position);
    }
}