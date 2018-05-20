using CongressCompanion.ClassObjects;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CongressCompanion
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
    {
        public MainPage()
        {
            InitializeComponent();

            MasterPage.ListView.ItemSelected += ListView_ItemSelected;

            //Set The Default Page
            MasterPage.ListView.SelectedItem = AppManager.Instance.AllTabPages[0];
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            //Get The Current Selected Item *IF* It Exists
            var item = e.SelectedItem as MainPageMenuItem;
            if (item == null)
            {
                return;
            }

            //Set The Detail Page Info
            Detail = AppManager.Instance.GetPageByName(item.Title);
            IsPresented = false;

            //Reset Selected Item
            MasterPage.ListView.SelectedItem = null;
        }
    }
}