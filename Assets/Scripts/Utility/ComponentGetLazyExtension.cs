using System;
using UnityEngine;

namespace Utility {
    public static class ComponentGetLazyExtension {
        public static T GetComponentLazy<T>(this Component component, ref T backingField) where T : Component {
            if (backingField == null) backingField = component.GetComponent<T>();
            return backingField;
        }        
    }
}