using AE_Xamarin.Misc;
using Xamarin.Forms;

namespace AE_Xamarin.Forms.Controls
{
    public class PhoneNumEntry : ValidationEntry
    {
        //Keep Track Of The Text From Before
        private string LastText = "";
        private int NumTextLength
        {
            get
            {
                string Temp = Text.Replace("(", "");
                Temp = Temp.Replace(")", "");
                Temp = Temp.Replace("-", "");
                Temp = Temp.Replace("+", "");
                return Temp.Length;
            }
        }

        //Ignore Case For Text Change
        private bool IgnoreTextChange = false;

        //The Length Of The Country Code
        private short CountryCodeLength = 0;

        public PhoneNumEntry() : base(BasicRegex.RegexPatters.PhoneValidation)
        {
            //Set Default Info
            Keyboard = Keyboard.Telephone;
            Placeholder = "(818)123-4657";

            //Add The Event To Change Number
            TextChanged += PhoneNumEntry_TextChanged;
        }
        private void PhoneNumEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Move On From This Text Change
            if (IgnoreTextChange)
            {
                IgnoreTextChange = false;
                return;
            }

            //Reset Country Code
            if(Text.Length == 0)
            {
                CountryCodeLength = 0;
                return;
            }

            //Make Sure There Is No Country Code
            if (Text.StartsWith("+"))
            {
                //Find Where The Country Code Ends
                if ((Text.Contains("(") || NumTextLength == 3) && CountryCodeLength == 0)
                {
                    //Set The Country Code Length
                    CountryCodeLength = (short)NumTextLength;

                    //Add The Parenthesis If It Is 3 Automatically
                    if (NumTextLength == 3)
                    {
                        Text += "(";
                        IgnoreTextChange = true;
                    }
                }
                else if(CountryCodeLength == 0)
                {
                    return;
                }
            }

            //Add Parenthesis
            if (NumTextLength == (3 + CountryCodeLength) && Text.Length == (3 + CountryCodeLength))
            {
                //(818)
                Text = $"({Text})";
                IgnoreTextChange = true;
            }

            //Add Parenthesis For Country Codes
            if(CountryCodeLength > 0 && Text.Length - Text.IndexOf('(') == 4 && LastText.Length < Text.Length)
            {
                Text += ")";
                IgnoreTextChange = true;
            }

            //Remove Parenthesis
            if (Text.Contains("(") && !Text.Contains(")") && CountryCodeLength == 0)
            {
                Text = Text.Replace("(", "");
                IgnoreTextChange = true;
            }

            //Add The Hyphen
            if (NumTextLength == (6 + CountryCodeLength) && LastText.Length < Text.Length && !Text.Contains("-"))
            {
                Text += '-';
                IgnoreTextChange = true;
            }
            
            //Save The Last Text
            LastText = Text;
        }
    }
}
