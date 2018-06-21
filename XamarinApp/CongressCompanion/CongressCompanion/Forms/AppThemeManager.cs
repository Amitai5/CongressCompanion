using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AE_Xamarin.Forms
{
    public class AppThemeManager
    {
        #region Singleton
        public static AppThemeManager Instance
        {
            get
            {
                lock (ManagerLock)
                {
                    if (ClassInstance == null)
                    {
                        throw new Exception("AppManager Has Not Yet Been Created!");
                    }
                    return ClassInstance;
                }
            }
        }
        private static AppThemeManager ClassInstance = null;
        private static readonly object ManagerLock = new object();

        private AppThemeManager(Dictionary<string, AppTheme> Themes)
        {
            //Set Themes
            AppThemes = Themes;
            SelectedTheme = ThemeNames[0];
        }
        #endregion Singleton

        public string CurrentThemeName
        {
            get
            {
                return SelectedTheme;
            }
        }
        public AppTheme CurrentTheme
        {
            get
            {
                return AppThemes[SelectedTheme];
            }
        }
        private string SelectedTheme;

        /// <summary>
        /// Selects The Theme To Display.
        /// </summary>
        /// <param name="ThemeName">The Theme Name.</param>
        public void SelectTheme(string ThemeName)
        {
            //Make Sure It Exists
            if(!AppThemes.ContainsKey(ThemeName))
            {
                throw new KeyNotFoundException($"The Specified Theme Name, {ThemeName} Could Not Be Located.");
            }

            //Set Theme
            SelectedTheme = ThemeName;

            //Check If Event Exists
            if (AppThemeChange != null)
            {
                //Update Event
                AppThemeChange.Invoke(this, new ThemeChangeArgs(ThemeName, CurrentTheme));
            }
        }
        private Dictionary<string, AppTheme> AppThemes;

        /// <summary>
        /// The Names Of All Themes Currently Loaded.
        /// </summary>
        public string[] ThemeNames
        {
            get
            {
                return AppThemes.Keys.ToArray();
            }
        }

        /// <summary>
        /// Fired Every Time The Theme Changes.
        /// </summary>
        public event EventHandler<ThemeChangeArgs> AppThemeChange;

        public static void Create(Dictionary<string, AppTheme> Themes)
        {
            //Make Sure It Has Not Yet Been Created
            if (ClassInstance != null)
            {
                throw new Exception("The AppManager Has Already Been Created!");
            }

            //Make Sure There Are Themes
            if(Themes == null || Themes.Count == 0)
            {
                throw new Exception("The Themes Cannot Be Null And Must Have At Least One Value!");
            }
            
            //Create The Instance
            ClassInstance = new AppThemeManager(Themes);
        }
    }
}