using System;

namespace Aurora.SMS.Blazor.Utils
{
    public static class AppState
    {
        private static string _currentTitle;

        public static string CurrentTitle
        {
            get
            {
                return _currentTitle;
            }
            set
            {
                _currentTitle = value;
                NotifyStateChanged();
            }
        }

        public static event Action OnChange;

        private static void NotifyStateChanged() => OnChange?.Invoke();
    }
}