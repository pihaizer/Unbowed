using System.Linq;
using UnityEngine;

namespace Unbowed.UI {
    public class MenuContainer : Menu {
        [SerializeField] Menu[] menus;

        protected override void Start() {
            base.Start();
            foreach (var menu in menus) {
                if (!menu) {
                    Debug.LogError($"Null menu in {gameObject}");
                    return;
                }
                
                menu.IsOpened.Changed += IsOpenedOnChanged;
            }
        }

        void IsOpenedOnChanged(bool obj) {
            SetOpened(menus.Any(menu => menu.IsOpened));
        }
    }
}