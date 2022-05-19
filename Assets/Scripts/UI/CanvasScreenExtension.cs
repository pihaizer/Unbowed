using System;
using HyperCore.UI;
using Unbowed.UI.Signals;
using Zenject;

namespace Unbowed.UI
{
    public static class CanvasScreenExtension
    {
        public static void SubscribeToAction(this CanvasScreen screen, 
            SignalBus bus, string screenName, Action<object> dataAction = null)
        {
            bus.Subscribe<ScreenActionSignal>(signal =>
            {
                if (signal.ScreenName != screenName) return;
                screen.ApplyScreenAction(signal.ScreenAction);
                dataAction?.Invoke(signal.Data);
            });
        }

        public static void ApplyScreenAction(this CanvasScreen screen, ScreenAction action)
        {
            switch (action)
            {
                case ScreenAction.Open:
                    screen.Open();
                    break;
                case ScreenAction.Close:
                    screen.Close();
                    break;
                case ScreenAction.Switch:
                    screen.Switch();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }
    }
}