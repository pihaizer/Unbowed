using UnityEditor;
using UnityEngine.Assertions;

namespace Gameplay.Editor {
    [CustomEditor(typeof(Character), true), CanEditMultipleObjects]
    public class CharacterEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            if (targets.Length > 1) return;
            
            var character = target as Character;
            Assert.IsNotNull(character);
            EditorGUILayout.LabelField($"Current command: {character.CurrentCharacterCommand}");
        }
    }
}