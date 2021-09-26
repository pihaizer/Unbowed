namespace Gameplay.Commands {
    public class IdleCommand : Command {
        float _timeRemaining;

        public IdleCommand(float time) {
            _timeRemaining = time;
        }

        public override void Update(float deltaTime) {
            base.Update(deltaTime);
            _timeRemaining -= deltaTime;
            if (_timeRemaining <= 0) {
                Stop(true);
            }
        }

        public override void Start(Character character) { }

        public override string ToString() => $"Idle ";
    }
}