namespace Unbowed.Signals
{
    public class LoadingScreenRequestSignal
    {
        public bool IsOpen;

        public LoadingScreenRequestSignal(bool isOpen)
        {
            IsOpen = isOpen;
        }
    }
}