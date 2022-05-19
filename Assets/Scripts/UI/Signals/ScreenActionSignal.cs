using System;

namespace Unbowed.UI.Signals
{
    public class ScreenActionSignal
    {
        public string ScreenName;
        public ScreenAction ScreenAction;
        public object Data;
        
        public ScreenActionSignal(string screenName, ScreenAction screenAction, object data = null)
        {
            ScreenName = screenName;
            ScreenAction = screenAction;
            Data = data;
        }
    }

    public enum ScreenAction
    {
        Open,
        Close,
        Switch
    }
}