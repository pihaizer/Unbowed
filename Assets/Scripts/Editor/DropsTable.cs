using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using Unbowed.Gameplay.Characters.Configs;
using UnityEditor;
using UnityEngine;

namespace Unbowed.Editor {
    public class DropsTable : OdinEditorWindow {
        [MenuItem("Tools/Drops table")]
        static void Open() {
            var window = GetWindow<DropsTable>();
            window.Show();
        }

        protected override void DrawEditor(int index) {
            base.DrawEditor(index);
        }

        protected override void OnEnable() {
            base.OnEnable();
            drops.Clear();
            string[] configGUIDs = AssetDatabase.FindAssets("t:CharacterConfig");
            foreach (string guid in configGUIDs) {
                var asset = AssetDatabase.LoadAssetAtPath<CharacterConfig>(AssetDatabase.GUIDToAssetPath(guid));
                drops.Add(new DropRow {name = asset.name, config = asset.dropsConfig});
            }
        }

        [SerializeField, Searchable]
        List<DropRow> drops = new List<DropRow>();

        [Serializable, InlineProperty]
        class DropRow {
            [Title("$name")]
            public DropsConfig config;
            
            [ReadOnly, HideLabel]
            public string name;
        }
    }
}