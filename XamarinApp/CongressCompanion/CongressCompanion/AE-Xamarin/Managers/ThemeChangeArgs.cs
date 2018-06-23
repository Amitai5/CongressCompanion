using System;

namespace AE_Xamarin.Managers
{
    public class ThemeChangeArgs : EventArgs
    {
        /// <summary>
        /// The Theme Name.
        /// </summary>
        public string ThemeName { get; private set; }

        /// <summary>
        /// The Updated Theme.
        /// </summary>
        public AppTheme NewTheme { get; private set; }

        public ThemeChangeArgs(string Name, AppTheme UpdatedTheme)
        {
            ThemeName = Name;
            NewTheme = UpdatedTheme;
        }
    }
}
