using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AE_Xamarin.Forms
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
