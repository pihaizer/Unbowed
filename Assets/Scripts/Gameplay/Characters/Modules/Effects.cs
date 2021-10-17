using System;
using System.Collections.Generic;

using Unbowed.Gameplay.Characters.Effects;

using UnityEngine;

namespace Unbowed.Gameplay.Characters.Modules {
    public class Effects : MonoBehaviour {
        readonly List<IEffect> _effects = new List<IEffect>();
        
        public event Action<IEffect> Added;
        public event Action<IEffect> Removed;

        void Update() {
            for (int i = _effects.Count - 1; i >= 0; i--) _effects[i].Update();
        }

        void FixedUpdate() {
            for (int i = _effects.Count - 1; i >= 0; i--) _effects[i].FixedUpdate();
        }

        public void Add(IEffect effect) {
            _effects.Add(effect);
            Added?.Invoke(effect);
        }

        public void Remove(IEffect effect) {
            _effects.Remove(effect);
            Removed?.Invoke(effect);
        }
    }
}