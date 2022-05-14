using System.Linq;
using UnityEngine;

namespace Unbowed.UI {
    public class MenuContainer : Menu {
        [SerializeField] private Menu[] menus;

        protected override void Awake() {
            base.Awake();
            foreach (var menu in menus) {
                menu.IsOpened.Changed += (value) => IsOpenedOnChanged(menu, value);
            }
        }

        private void IsOpenedOnChanged(Menu changedMenu, bool value) {
            if (!value) {
                Close();
                return;
            }
            
            foreach (var menu in menus) {
                if (menu == changedMenu) continue;
                menu.Close();
            }
            
            Open();
        }
    }
}