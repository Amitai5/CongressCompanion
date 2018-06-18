using System;

namespace CongressCompanion.ClassObjects
{
    public class Representative : IComparable
    {
        public Uri Website { get; private set; }
        public Uri ImageUrl { get; private set; }
        public string FullName { get; private set; }
        public string GovLevel { get; private set; }
        public string OfficeName { get; private set; }
        public string PhoneNumber { get; private set; }
        public string DivisionID { get; private set; }
        public string PrimaryEmail { get; private set; }
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
                OfficeName = officeName;
                DivisionID = divisionID;
                GovLevel = levelName;
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