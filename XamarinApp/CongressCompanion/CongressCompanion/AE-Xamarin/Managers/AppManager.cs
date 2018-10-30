using CongressCompanion;
using CongressCompanion.ClassObjects;
using CongressCompanion.MasterPageViews;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AE_Xamarin.Managers
{
    public class AppManager
    {
        #region Singleton Code
        public static AppManager Instance
        {
            get
            {
                lock (ManagerLock)
                {
                    if (ClassInstance == null)
                    {
                        ClassInstance = new AppManager();
                    }
                    return ClassInstance;
                }
            }
        }

        private static AppManager ClassInstance = null;
        private static readonly object ManagerLock = new object();
        #endregion Singleton Code

        #region Save Data Reflection Code

        /// <summary>
        /// Gets The List Of Objects That Contains The Save Atribute.
        /// </summary>
        /// <param name="atype">The Main Class To Save Items From.</param>
        /// <returns>The List Of Objects To Be Saved.</returns>
        private static Dictionary<string, object> GetSaveItems(object atype)
        {
            //Check That They Pass In An Object
            if (atype == null)
            {
                return new Dictionary<string, object>();
            }

            //Get The Main Class Type
            Type ClassType = atype.GetType();

            //Get All The Fields Of The Class With The Save Attribute
            Dictionary<string, object> TempDictionary = new Dictionary<string, object>();
            FieldInfo[] Members = ClassType.GetFields().Where(prop => Attribute.IsDefined(prop, typeof(SaveItemAttribute))).ToArray();

            //Go Through Them All And Add Them To The Return List
            foreach (MemberInfo Member in Members)
            {
                TempDictionary.Add(((FieldInfo)Member).Name, ((FieldInfo)Member).GetValue(Instance));
            }
            return TempDictionary;
        }
        private Dictionary<string, object> SaveItems;

        /// <summary>
        /// Resets The Values Of Items That Are Tagged With The "SaveItem Attribute" To Their Defaults.
        /// THIS CANNOT BE REVERSED!!
        /// </summary>
        public void ResetSaveData()
        {
            //Try To Get Save Items
            SaveItems = GetSaveItems(this);

            //Check For Any Save Items
            if (SaveItems == null)
            {
                return;
            }

            //Load Variable Values To Their Stored Defaults
            FieldInfo[] Members = typeof(AppManager).GetFields().Where(prop => Attribute.IsDefined(prop, typeof(SaveItemAttribute))).ToArray();
            foreach (FieldInfo Member in Members)
            {
                //Get Default Value As Object
                object DefaultValue = Member.GetCustomAttribute<SaveItemAttribute>().DefaultValue;

                //Set Variable To Default Values
                Member.SetValue(Instance, DefaultValue);
            }

            //Reset The Saved Theme To Default
            AppThemeManager.Instance.SelectTheme(AppThemeManager.Instance.ThemeNames[0]);

            //Save The Default Version
            SaveData();
        }

        /// <summary>
        /// Loads Data That Is Tagged With The "SaveItem Attribute".
        /// </summary>
        public void LoadSavedData()
        {
            //Try To Get Save Items
            SaveItems = GetSaveItems(this);

            //Check For Any Save Items
            if (SaveItems == null)
            {
                return;
            }

            //Load Up The Saved Values
            string[] ItemNames = SaveItems.Keys.ToArray();
            for (int i = 0; i < ItemNames.Length; i++)
            {
                if (Application.Current.Properties.ContainsKey(ItemNames[i]))
                {
                    SaveItems[ItemNames[i]] = Application.Current.Properties[ItemNames[i]];
                }
            }

            //Load Variable Values
            FieldInfo[] Members = typeof(AppManager).GetFields().Where(prop => Attribute.IsDefined(prop, typeof(SaveItemAttribute))).ToArray();
            foreach (FieldInfo Member in Members)
            {
                //Set Variable Values
                Member.SetValue(Instance, SaveItems[Member.Name]);
            }

            //Check If The Theme Has Been Saved Before
            if (Application.Current.Properties.ContainsKey("SavedThemeName"))
            {
                if (Application.Current.Properties["SavedThemeName"] != null)
                {
                    //Set The CurrentTheme
                    string ThemeName = Application.Current.Properties["SavedThemeName"].ToString();
                    AppThemeManager.Instance.SelectTheme(ThemeName);
                }
            }
            else
            {
                //Set The Theme To Default
                Application.Current.Properties.Add("SavedThemeName", null);
            }
        }

        /// <summary>
        /// Saves The Data That Is Tagged With The "SaveItem Attribute".
        /// </summary>
        public void SaveData()
        {
            //Reload Objects With Their New Values
            SaveItems = GetSaveItems(this);

            //Save The Objects
            string[] ItemNames = SaveItems.Keys.ToArray();
            for (int i = 0; i < ItemNames.Length; i++)
            {
                Application.Current.Properties[ItemNames[i]] = SaveItems[ItemNames[i]];
            }

            //Save The CurrentTheme
            Application.Current.Properties["SavedThemeName"] = AppThemeManager.Instance.CurrentThemeName;

            //Save The Application Properties
            Application.Current.SavePropertiesAsync();
        }

        #endregion Save Data Reflection Code

        private AppManager()
        {
            //Set All Tab Pages
            AllTabPages = new ObservableCollection<MainPageMenuItem>(new[]
                {
                    new MainPageMenuItem { Id = 0, Title = "Federal" },
                    new MainPageMenuItem { Id = 1, Title = "State" },
                    new MainPageMenuItem { Id = 2, Title = "Local" },
                    new MainPageMenuItem { Id = 3, Title = "Settings" }
                });

            //Set Themes
            Dictionary<string, AppTheme> Themes = new Dictionary<string, AppTheme>()
            {
                //Standard Themes
                { "Default", new AppTheme("#F9F3EB", "#CF1942", "#1D407C") },
                { "Dark", new AppTheme("#284472", "#3A3337", "#1A1A1A" ) },

                { "Halloween", new AppTheme("#F26430", "#3A3337", "#1A1A1A" ) }
            };
            AppThemeManager.Create(Themes);
        }

        /// <summary>
        /// Store Their Current Search Location.
        /// </summary>
        public string UserLocationInfo
        {
            get
            {
                return LocationInfo;
            }
            set
            {
                //Store If Zipcode
                IsZipcode = BasicRegex.IsValidZipcode(value);
                LocationInfo = value;
            }
        }

        [SaveItem(true)] //Save Location
        public bool ShouldSaveLocation;

        [SaveItem(null)] //Save Location Info
        public string LocationInfo;

        /// <summary>
        /// Store The Current Info Of What Location Type Is Saved.
        /// </summary>
        public bool IsZipcode = false;

        /// <summary>
        /// The List Of Representatives For The Government Offices In The Federal Government.
        /// </summary>
        public List<Representative> FederalReps = new List<Representative>();

        /// <summary>
        /// The List Of Representatives For The Government Offices In The State Government.
        /// </summary>
        public List<Representative> StateReps = new List<Representative>();

        /// <summary>
        /// The List Of Representatives For The Government Offices In The District Government.
        /// </summary>
        public List<Representative> LocalReps = new List<Representative>();

        /// <summary>
        /// Loads The Local Congress Info.
        /// </summary>
        public async Task<bool> LoadRepresentatives()
        {
            //Clear All Data
            LocalReps.Clear();
            StateReps.Clear();
            FederalReps.Clear();

            //Get Json Data
            string RepDataJson = await GetRepData();

            //Check If It Failed
            if (RepDataJson == null)
            {
                return false;
            }

            //Load Up The Reps
            List<Representative> TempRepList = new List<Representative>();
            Rootobject JsonData = JsonConvert.DeserializeObject<Rootobject>(RepDataJson);
            for (int RepIndex = 0; RepIndex < JsonData.officials.Length; RepIndex++)
            {
                Official CurrentOfficial = JsonData.officials[RepIndex];
                TempRepList.Add(new Representative(CurrentOfficial.urls, CurrentOfficial.photoUrl, CurrentOfficial.name, CurrentOfficial.phones, CurrentOfficial.emails, CurrentOfficial.party));
            }

            //Load Up Gov Positions
            for (int officeIndex = 0; officeIndex < JsonData.offices.Length; officeIndex++)
            {
                for (int i = 0; i < JsonData.offices[officeIndex].officialIndices.Length; i++)
                {
                    int OfficialIndex = JsonData.offices[officeIndex].officialIndices[i];
                    string[] OfficeLevels = JsonData.offices[officeIndex].levels;
                    if (OfficeLevels != null && OfficeLevels.Length > 0)
                    {
                        TempRepList[OfficialIndex].SetOfficeData(officeIndex, JsonData.offices[officeIndex].name, JsonData.offices[officeIndex].divisionId, OfficeLevels[0]);
                    }
                    else
                    {
                        TempRepList[OfficialIndex].SetOfficeData(officeIndex, JsonData.offices[officeIndex].name, JsonData.offices[officeIndex].divisionId, "");
                    }
                }
            }

            //Sort Gov Reps
            SortAllReps(TempRepList);
            return true;
        }

        /// <summary>
        /// The API Call Format.
        /// </summary>
        private const string APICallFormat = "https://www.googleapis.com/civicinfo/v2/representatives?address={0}&key={1}";
        private void SortAllReps(List<Representative> TempRepList)
        {
            //Sort By Gov Level
            foreach (Representative CurrentRep in TempRepList)
            {
                //Check For Federal Rep
                switch (CurrentRep.GovLevel)
                {
                    case RepGovLevel.Country:
                        FederalReps.Add(CurrentRep);
                        break;
                    case RepGovLevel.State:
                        StateReps.Add(CurrentRep);
                        break;
                    case RepGovLevel.Place:
                    case RepGovLevel.County:
                        LocalReps.Add(CurrentRep);
                        break;
                }
            }

            //Sort The Others Alphabetically
            StateReps.Sort();
            LocalReps.Sort();
        }
        private async Task<string> GetRepData()
        {
            //Get Info
            HttpClient Client = new HttpClient();
            string CallPath = string.Format(APICallFormat, UserLocationInfo, "AIzaSyDi4NE7X-itw0B8qu9Fk-VtN9dSSZybMLE");
            string RepDataJson = await Client.GetStringAsync(CallPath);

            //Send Back Data
            bool SuccessfulCall = !RepDataJson.Contains("error");
            return SuccessfulCall ? RepDataJson : null;
        }

        #region NavigationInfo

        /// <summary>
        /// Gets The Page Based Off Of The Page Name.
        /// </summary>
        /// <param name="PageName">The Name Of The Current Selected Page.</param>
        /// <returns>The Page That Corresponds With The Page Name.</returns>
        public NavigationPage GetPageByName(string PageName)
        {
            //Create The Correct Content Page
            ContentPage EmbededPage = null;
            switch (PageName.ToLower())
            {
                case "federal":
                    EmbededPage = new RepsListPage(RepLoadType.Federal);
                    break;
                case "state":
                    EmbededPage = new RepsListPage(RepLoadType.State);
                    break;
                case "local":
                    EmbededPage = new RepsListPage(RepLoadType.Local);
                    break;
                case "settings":
                    EmbededPage = new SettingsPage();
                    break;
                default:
                    EmbededPage = new ContentPage();
                    break;
            }

            //Edit The Page Heading & Nav Bar
            EmbededPage.Title = PageName;

            //Create The Navigation Page
            NavigationPage ReturnPage = new NavigationPage(EmbededPage)
            {
                BarBackgroundColor = AppThemeManager.Instance.CurrentTheme.NavBarColor,
                BackgroundColor = AppThemeManager.Instance.CurrentTheme.BackgroundColor,
                BarTextColor = AppThemeManager.Instance.CurrentTheme.TextColor
            };

            //Return The Page
            return ReturnPage;
        }

        /// <summary>
        /// The Total Collection Of All MainPageMenu Tabs.
        /// </summary>
        public ObservableCollection<MainPageMenuItem> AllTabPages { private set; get; }

        #endregion NavigationInfo
    }
}
