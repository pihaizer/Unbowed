using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Utility {
    public static class DeepCopyExtension {
        static readonly MethodInfo CloneMethod = typeof(object).GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance);

        public static bool IsPrimitive(this Type type) {
            if (type == typeof(String)) return true;
            return (type.IsValueType & type.IsPrimitive);
        }

        public static object DeepCopy(this object originalDestroyable) {
            return InternalCopy(originalDestroyable, new Dictionary<object, object>(new ReferenceEqualityComparer()));
        }

        static object InternalCopy(object originalDestroyable, IDictionary<object, object> visited) {
            if (originalDestroyable == null) return null;
            var typeToReflect = originalDestroyable.GetType();
            if (IsPrimitive(typeToReflect)) return originalDestroyable;
            if (visited.ContainsKey(originalDestroyable)) return visited[originalDestroyable];
            if (typeof(Delegate).IsAssignableFrom(typeToReflect)) return null;
            var cloneObject = CloneMethod.Invoke(originalDestroyable, null);
            if (typeToReflect.IsArray) {
                var arrayType = typeToReflect.GetElementType();
                if (IsPrimitive(arrayType) == false) {
                    Array clonedArray = (Array)cloneObject;
                    clonedArray.ForEach((array, indices) => array.SetValue(InternalCopy(clonedArray.GetValue(indices), visited), indices));
                }

            }
            visited.Add(originalDestroyable, cloneObject);
            CopyFields(originalDestroyable, visited, cloneObject, typeToReflect);
            RecursiveCopyBaseTypePrivateFields(originalDestroyable, visited, cloneObject, typeToReflect);
            return cloneObject;
        }

        static void RecursiveCopyBaseTypePrivateFields(object originalObject, IDictionary<object, object> visited, object cloneObject, Type typeToReflect) {
            if (typeToReflect.BaseType != null) {
                RecursiveCopyBaseTypePrivateFields(originalObject, visited, cloneObject, typeToReflect.BaseType);
                CopyFields(originalObject, visited, cloneObject, typeToReflect.BaseType, BindingFlags.Instance | BindingFlags.NonPublic, info => info.IsPrivate);
            }
        }

        static void CopyFields(object originalObject, IDictionary<object, object> visited, object cloneObject, Type typeToReflect, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy, Func<FieldInfo, bool> filter = null) {
            foreach (FieldInfo fieldInfo in typeToReflect.GetFields(bindingFlags)) {
                if (filter != null && filter(fieldInfo) == false) continue;
                if (IsPrimitive(fieldInfo.FieldType)) continue;
                var originalFieldValue = fieldInfo.GetValue(originalObject);
                var clonedFieldValue = InternalCopy(originalFieldValue, visited);
                fieldInfo.SetValue(cloneObject, clonedFieldValue);
            }
        }
        public static T Copy<T>(this T original) {
            return (T)DeepCopy((object)original);
        }
    }

    public class ReferenceEqualityComparer : EqualityComparer<object> {
        public override bool Equals(object x, object y) {
            return ReferenceEquals(x, y);
        }
        public override int GetHashCode(object obj) {
            if (obj == null) return 0;
            return obj.GetHashCode();
        }
    }

    public static class ArrayExtensions {
        public static void ForEach(this Array array, Action<Array, int[]> action) {
            if (array.LongLength == 0) return;
            ArrayTraverse walker = new ArrayTraverse(array);
            do action(array, walker.Position);
            while (walker.Step());
        }
    }

    internal class ArrayTraverse {
        public int[] Position;
        int[] maxLengths;

        public ArrayTraverse(Array array) {
            maxLengths = new int[array.Rank];
            for (int i = 0; i < array.Rank; ++i) {
                maxLengths[i] = array.GetLength(i) - 1;
            }
            Position = new int[array.Rank];
        }

        public bool Step() {
            for (int i = 0; i < Position.Length; ++i) {
                if (Position[i] < maxLengths[i]) {
                    Position[i]++;
                    for (int j = 0; j < i; j++) {
                        Position[j] = 0;
                    }
                    return true;
                }
            }
            return false;
        }
    }
}