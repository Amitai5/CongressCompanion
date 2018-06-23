using AE_Xamarin.Managers;
using CongressCompanion.MasterPageViews;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace CongressCompanion
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPageMaster : ContentPage
    {
        public ListView ListView;

        public MainPageMaster()
        {
            InitializeComponent();

            UpdateThemeColors();
            HeaderBar.Color = Color.FromHex("#ECAB66");
            AppThemeManager.Instance.AppThemeChange += Instance_AppThemeChange;

            //Set The MainPageMasterView
            BindingContext = new MainPageMasterViewModel();
            ListView = MenuItemsListView;
        }
        private void UpdateThemeColors()
        {
            PageHeader.TextColor = AppThemeManager.Instance.CurrentTheme.TextColor;
            BackgroundColor = AppThemeManager.Instance.CurrentTheme.BackgroundColor;
        }
        private void Instance_AppThemeChange(object sender, ThemeChangeArgs e)
        {
            UpdateThemeColors();
        }
        
        [Preserve(AllMembers = true)]
        class MainPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MainPageMenuItem> MenuItems { get; set; }

            public MainPageMasterViewModel()
            {
                MenuItems = AppManager.Instance.AllTabPages;
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}