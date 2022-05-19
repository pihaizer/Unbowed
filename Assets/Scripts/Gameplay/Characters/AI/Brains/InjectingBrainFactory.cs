using Unbowed.SO.Brains;
using Zenject;

namespace Unbowed.Gameplay.Characters.AI.Brains
{
    public class InjectingBrainFactory : IFactory<BrainConfigSO, Brain>
    {
        [Inject] private DiContainer _container;

        public Brain Create(BrainConfigSO brainConfig)
        {
            Brain brain = brainConfig.Create();
            _container.Inject(brain);
            return brain;
        }
    }
}