using System.Linq;
using Sirenix.OdinInspector.Demos.RPGEditor;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using Unbowed.Gameplay.Characters.Configs;
using UnityEditor;
using UnityEngine;

namespace Unbowed.Editor {
    public class CharactersOverview : OdinMenuEditorWindow {
        [MenuItem("Tools/Characters overview")]
        static void Open() {
            var window = GetWindow<CharactersOverview>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
        }

        protected override OdinMenuTree BuildMenuTree() {
            var tree = new OdinMenuTree(true) {
                Config = {
                    DrawSearchToolbar = true
                }
            };

            tree.AddAllAssetsAtPath("", "Assets/Configs/Characters", typeof(CharacterConfig), true);

            return tree;
        }

        protected override void OnBeginDrawEditors()
        {
            var selected = this.MenuTree.Selection.FirstOrDefault();
            var toolbarHeight = this.MenuTree.Config.SearchToolbarHeight;

            // Draws a toolbar with the name of the currently selected menu item.
            SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
            {
                if (selected != null)
                {
                    GUILayout.Label(selected.Name);
                }

                // if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create Item")))
                // {
                //     ScriptableObjectCreator.ShowDialog<Item>("Assets/Plugins/Sirenix/Demos/Sample - RPG Editor/Items", obj =>
                //     {
                //         obj.Name = obj.name;
                //         base.TrySelectMenuItemWithObject(obj); // Selects the newly created item in the editor
                //     });
                // }

                if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create Character")))
                {
                    ScriptableObjectCreator.ShowDialog<CharacterConfig>("Assets/Configs/Characters", obj =>
                    {
                        base.TrySelectMenuItemWithObject(obj); // Selects the newly created item in the editor
                    });
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }
    }
}