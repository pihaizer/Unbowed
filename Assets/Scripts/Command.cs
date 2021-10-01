﻿using System;
using UnityEngine;

namespace Unbowed {
    public abstract class Command {
        public event Action<bool> Executed;
        
        public bool Result { get; private set; }

        public virtual void Start() { }

        public virtual void Update(float deltaTime) { }

        public virtual void Stop(bool result) {
            Result = result;
            Executed?.Invoke(result);
        }
    }
}