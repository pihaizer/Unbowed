using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Unbowed {
    public class CommandExecutor : MonoBehaviour {
        public event Action<Command> StartedExecuting;
        public event Action<Command> StoppedExecuting;

        public Command MainCommand { get; private set; }
        
        [ShowInInspector, PropertyOrder(1), DisplayAsString]
        private string CurrentCommandString => MainCommand?.ToString();

        [ShowInInspector, PropertyOrder(2)]
        public readonly List<Command> additionalCommands = new List<Command>();

        public void FixedUpdate() {
            MainCommand?.Update(Time.fixedDeltaTime);
            foreach (var additionalCommand in additionalCommands) {
                additionalCommand.Update(Time.fixedDeltaTime);
            }
        }

        public virtual void Execute(Command command, bool isMain = true) {
            ForceExecute(command, isMain);
        }

        public virtual void ForceExecute(Command command, bool isMain = true) {
            if (isMain) {
                MainCommand?.Stop(false);
                MainCommand = command;
            }

            command.Executed += (result) => OnCommandExecuted(command, result);
            StartedExecuting?.Invoke(command);
            command.Start();
        }

        public void Stop() {
            StopMain();
            for (int i = additionalCommands.Count - 1; i >= 0; i--) {
                additionalCommands[i].Stop(false);
            }
        }

        public void StopMain() => MainCommand?.Stop(false);

        private void OnCommandExecuted(Command command, bool result) {
            if (additionalCommands.Contains(command)) {
                additionalCommands.Remove(command);
            } else {
                MainCommand = null;
            }
            StoppedExecuting?.Invoke(command);
        }
    }
}