using System;

namespace CongressCompanion.ClassObjects
{
    public class Representative : IComparable
    {
        public Uri Website { get; private set; }
        public Uri ImageUrl { get; private set; }
        public string FullName { get; private set; }
        public string OfficeName { get; private set; }
        public string DivisionID { get; private set; }
        public string PhoneNumber { get; private set; }
        public string PrimaryEmail { get; private set; }
        public RepGovLevel GovLevel { get; private set; }
        public PoliticalParty Party { get; private set; }

        //Store Their Rank
        private int RepOfficeIndex = -1;

        public Representative(string[] websites, string imageUrl, string fullName, string[] phoneNumbers, string[] emails, string party)
        {
            FullName = fullName;

            //Check For No Image
            if (!string.IsNullOrEmpty(imageUrl))
            {
                ImageUrl = new Uri(imageUrl);
            }

            //Check For No Phone Numbers
            if (phoneNumbers != null && phoneNumbers.Length > 0)
            {
                PhoneNumber = phoneNumbers[0];
            }

            //Check For No Websites
            if (websites != null && websites.Length > 0)
            {
                Website = new Uri(websites[0]);
            }

            //Check For No Emails
            if (emails != null && emails.Length > 0)
            {
                PrimaryEmail = emails[0];
            }

            //Check Political Party
            if (party != null)
            {
                int EnumCounter = 0;
                foreach (string PartyName in Enum.GetNames(typeof(PoliticalParty)))
                {
                    if (PartyName.ToLower() == party.ToLower())
                    {
                        Party = (PoliticalParty)EnumCounter;
                        return;
                    }
                    EnumCounter++;
                }
            }
        }
        public void SetOfficeData(int OfficeIndex, string officeName, string divisionID, string levelName)
        {
            //Set Office Rank
            RepOfficeIndex = OfficeIndex;

            //Check If It Exists Or Not Already
            if (string.IsNullOrEmpty(OfficeName))
            {
                //Set Variables
                DivisionID = divisionID;
                GovLevel = RepGovLevel.Unknown;

                //First Sort Out Levels
                if (levelName.ToLower().Trim() == "country")
                {
                    GovLevel = RepGovLevel.Country;
                }

                //Check The Division After The Level
                if (GovLevel == RepGovLevel.Unknown)
                {
                    if (divisionID.Contains("county"))
                    {
                        GovLevel = RepGovLevel.County;
                    }
                    else if (divisionID.Contains("place"))
                    {
                        GovLevel = RepGovLevel.Place;
                    }
                    else if (divisionID.Contains("state"))
                    {
                        GovLevel = RepGovLevel.State;
                    }
                    else
                    {
                        GovLevel = RepGovLevel.Country;
                    }
                }

                //Adjust Names
                string officeNameLowered = officeName.ToLower();
                OfficeName = officeName;
                switch (GovLevel)
                {
                    case RepGovLevel.State:
                        if (!officeNameLowered.Contains("state"))
                        {
                            OfficeName = $"State {officeName}";
                        }
                        break;
                    case RepGovLevel.County:
                        if (!officeNameLowered.Contains("district") && !officeNameLowered.Contains("county"))
                        {
                            OfficeName = $"County {officeName}";
                        }
                        break;
                    case RepGovLevel.Place:
                        if (!officeNameLowered.Contains("city"))
                        {
                            OfficeName = $"City {officeName}";
                        }
                        break;
                }
            }
        }

        public int CompareTo(object obj)
        {
            //Check Alphabetically
            Representative OtherRep = (Representative)obj;
            return RepOfficeIndex.CompareTo(OtherRep.RepOfficeIndex);
            //return string.Compare(FullName, OtherRep.FullName, true);
        }
    }
}
