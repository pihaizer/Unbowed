using System;
using Unbowed.Gameplay.Characters.Configs;
using Unbowed.Gameplay.Characters.Items;
using Unbowed.Gameplay.Signals;
using Unbowed.SO;
using Unbowed.Utility;
using Unbowed.Utility.Modifiers;
using UnityEngine;
using Zenject;

namespace Unbowed.Gameplay.Characters.Modules
{
    public class ExperienceModule : IDisposable
    {
        public Character Character { get; private set; }
        public ExperienceConfig Config { get; private set; }
        
        public readonly Mutable<int> Level = new();
        public readonly Mutable<long> Experience = new();

        private SignalBus _bus;

        public void Init(Character character, SignalBus bus)
        {
            _bus = bus;
            Character = character;
            Config = character.Config.ExperienceConfig;
            if (!Config.CanLevelUp) return;
            if(Level == 0) Level.Set(1);
            _bus.Subscribe<IExperienceGiver>(OnCharacterDied);
        }

        public void Dispose() => _bus.TryUnsubscribe<CharacterDiedSignal>(OnCharacterDied);

        private void OnCharacterDied(IExperienceGiver data)
        {
            AddExperience(data.ExperienceGained);
        }

        private bool IsCloseForExperience(Character other) =>
            Vector3.Distance(Character.transform.position, other.transform.position)
            < Config.ExperienceGainDistance;

        private void AddExperience(long amount)
        {
            Experience.Value += amount;
            TryLevelUp();
        }

        private void TryLevelUp()
        {
            if (Level == LevelingConfig.Instance.MaxLevel) return;
            while (Experience >= LevelingConfig.Instance.GetExperienceToLevelUp(Level + 1))
            {
                Experience.Value -= LevelingConfig.Instance.GetExperienceToLevelUp(Level + 1);
                Level.Value++;
                _bus.Fire(new CharacterLeveledUp(Character, Level));
            }
        }
    }
}