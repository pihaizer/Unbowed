using System.Linq;
using HyperCore.UI;
using UnityEngine;

namespace Unbowed.UI
{
    public class CanvasScreenContainer : CanvasScreen
    {
        [SerializeField] private CanvasScreen[] menus;

        protected override void Awake()
        {
            base.Awake();
            foreach (var menu in menus)
            {
                menu.Opened += () => OnScreenOpened(menu);
                menu.Closed += OnScreenClosed;
            }
        }

        public void CloseAll()
        {
            foreach (CanvasScreen menu in menus)
            {
                menu.Close();
            }
        }

        private void OnScreenClosed() => Close();

        private void OnScreenOpened(CanvasScreen changedScreen)
        {
            foreach (CanvasScreen menu in menus)
            {
                if (menu == changedScreen) continue;
                menu.Close();
            }

            Open();
        }
    }
}