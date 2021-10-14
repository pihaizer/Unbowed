using System;
using System.Collections.Generic;

using Unbowed.SO;

using UnityEngine;

namespace Unbowed.Managers {
    public class ConfigsContainer : MonoBehaviour {
        public List<ConfigSingleton> configs;

        public static ConfigsContainer Instance {
            get {
                if (!_instance) _instance = FindObjectOfType<ConfigsContainer>();
                return _instance;
            }
        }

        static ConfigsContainer _instance;
        
        void Awake() {
            _instance = this;
        }
    }
}