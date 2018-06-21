using AE_Xamarin.Forms;
using AE_Xamarin.Misc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using Xamarin.Forms;

namespace CongressCompanion.ClassObjects
{
    public class AppManager
    {
        #region All The Singleton Stuff
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
                { "Default", new AppTheme("#FDFCFB", "#E6335D", "#2B5797") },
                { "Dark", new AppTheme("#F9F3EB", "#CF1942", "#1D407C") }
            };
            AppThemeManager.Create(Themes);

            //Load User Data
            LoadDataFromSave();
        }
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
        #endregion All The Singleton Stuff

        /// <summary>
        /// Loads All Of The User's Prefereces From The Save.
        /// </summary>
        public void LoadDataFromSave()
        {
            //Check If Saved Before
            if (Application.Current.Properties.ContainsKey("ShouldSaveLocation"))
            {
                ShouldSaveLocation = Convert.ToBoolean(Application.Current.Properties["ShouldSaveLocation"]);
            }
            else
            {
                //If Not Saved Before, Then Create It
                Application.Current.Properties.Add("ShouldSaveLocation", ShouldSaveLocation);
            }

            //Check If Saved Before
            if (Application.Current.Properties.ContainsKey("UserLocationInfo"))
            {
                //Reload The Representatives From The Saved Location
                UserLocationInfo = (string)Application.Current.Properties["UserLocationInfo"];
            }
            else
            {
                //If Not Saved Before, Then Create It
                Application.Current.Properties.Add("UserLocationInfo", UserLocationInfo);
            }

            //Check If Saved Before
            if (Application.Current.Properties.ContainsKey("CurrentTheme"))
            {
                AppThemeManager.Instance.SelectTheme(Application.Current.Properties["CurrentTheme"].ToString());
            }
            else
            {
                //If Not Saved Before, Then Create It
                Application.Current.Properties.Add("CurrentTheme", AppThemeManager.Instance.CurrentThemeName);
            }
        }

        /// <summary>
        /// Saves All Of The User's Prefereces From Storage.
        /// </summary>
        public async void SaveUserData()
        {
            //Save The Location Boolean
            Application.Current.Properties["ShouldSaveLocation"] = ShouldSaveLocation;

            //Check If Location Should Be Saved
            if (ShouldSaveLocation)
            {
                Application.Current.Properties["UserLocationInfo"] = UserLocationInfo;
            }
            else
            {
                //If We Should Not Save... Clear The Data
                Application.Current.Properties["UserLocationInfo"] = null;
            }

            //Save The Current Theme
            Application.Current.Properties["CurrentTheme"] = AppThemeManager.Instance.CurrentThemeName;

            //Sync Save It
            await Application.Current.SavePropertiesAsync();
        }

        /// <summary>
        /// Resets All Of The User's Current Save Data
        /// </summary>
        public async void ResetSaveData()
        {
            //Reset The Location Boolean
            Application.Current.Properties["ShouldSaveLocation"] = true;
            ShouldSaveLocation = true;

            //Reset The Location Info
            Application.Current.Properties["UserLocationInfo"] = null;
            UserLocationInfo = null;
            
            //Reset The Current Theme
            Application.Current.Properties["CurrentTheme"] = AppThemeManager.Instance.ThemeNames[0];
            AppThemeManager.Instance.SelectTheme(AppThemeManager.Instance.ThemeNames[0]);

            //Sync Save It
            await Application.Current.SavePropertiesAsync();
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
        public bool ShouldSaveLocation;
        private string LocationInfo;

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
                if (CurrentRep.GovLevel.ToLower().Trim() == "country")
                {
                    FederalReps.Add(CurrentRep);
                }

                //Check For State Rep
                else if (!CurrentRep.DivisionID.Contains("county"))
                {
                    StateReps.Add(CurrentRep);
                }

                //Add The Local Reps Up
                else
                {
                    LocalReps.Add(CurrentRep);
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