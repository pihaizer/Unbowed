using Unbowed.Gameplay.Characters.Commands;
using Unbowed.SO.Brains;
using UnityEngine;
using Zenject;

namespace Unbowed.Gameplay.Characters.AI.Brains
{
    public abstract class Brain
    {
        public int Id { get; }

        protected Character Body;
        protected ColliderZone RestrictedZone = null;

        protected Brain(int id)
        {
            Id = id;
        }

        public virtual void SetBody(Character body)
        {
            Body = body;
        }

        public virtual void FixedUpdate()
        {
        }

        public virtual void Update()
        {
        }

        public virtual void OnDestroy()
        {
        }

        public void SetRestrictedZone(ColliderZone zone) => RestrictedZone = zone;

        protected void MoveToPoint(Vector3 point, float maxTime = float.MaxValue)
        {
            Body.commands.Execute(new MoveCommand(point, maxTime));
        }

        protected void Attack(IHittable hittable, float maxTime = float.MaxValue) =>
            Body.commands.Execute(new AttackCommand(hittable, maxTime));

        protected void Idle(float time) =>
            Body.commands.Execute(new IdleCommand(time));

        protected bool IsMovePositionValid(Vector3 position) =>
            RestrictedZone == null || RestrictedZone.Contains(position);

        public class Factory : PlaceholderFactory<Character, Brain>
        {
        }
    }
}