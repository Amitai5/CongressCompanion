using Xamarin.Forms;

namespace AE_Xamarin.Managers
{
    public class AppTheme
    {
        public Color TextColor { get; private set; }
        public Color NavBarColor { get; private set; }
        public Color BackgroundColor { get; private set; }

        /// <summary>
        /// Creates The AppTheme Class Using Known Colors.
        /// </summary>
        /// <param Name="TxtColor">The Text Color.</param>
        /// <param Name="NavColor">The Navbar Color.</param>
        /// <param Name="BkgdColor">The Background Color.</param>
        public AppTheme(Color TxtColor, Color NavColor, Color BkgdColor)
        {
            TextColor = TxtColor;
            NavBarColor = NavColor;
            BackgroundColor = BkgdColor;
        }

        /// <summary>
        /// Creates The AppTheme Class Using Hex Colors.
        /// </summary>
        /// <param Name="TxtColor">The Text Color In Hex.</param>
        /// <param Name="NavColor">The Navbar Color In Hex.</param>
        /// <param Name="BkgdColor">The Background Color In Hex.</param>
        public AppTheme(string TxtColor, string NavColor, string BkgdColor)
        {
            TextColor = Color.FromHex(TxtColor);
            NavBarColor = Color.FromHex(NavColor);
            BackgroundColor = Color.FromHex(BkgdColor);
        }
    }
}
