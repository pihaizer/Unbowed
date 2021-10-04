using System.Linq;
using UnityEngine;

namespace Unbowed.UI {
    public class MenuContainer : Menu {
        [SerializeField] Menu[] menus;

        protected override void Awake() {
            base.Awake();
            foreach (var menu in menus) {
                if (menu == this) return;
                menu.IsOpened.Changed += IsOpenedOnChanged;
            }
        }

        void IsOpenedOnChanged(bool obj) {
            SetOpened(menus.Any(menu => menu.IsOpened));
        }
    }
}