using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Unbowed
{
    public class ScriptableObjectsInstaller : MonoInstaller
    {
        [SerializeField] private List<ScriptableObject> injectableSOs;

        public override void InstallBindings()
        {
            foreach (ScriptableObject scriptableObject in injectableSOs)
            {
                Container.QueueForInject(scriptableObject);
            }
        }
    }
}
