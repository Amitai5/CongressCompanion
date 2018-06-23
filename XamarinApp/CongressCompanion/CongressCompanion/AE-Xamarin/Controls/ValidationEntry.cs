using AE_Xamarin.Managers;
using System;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace AE_Xamarin.Controls
{
    public class ValidationEntry : Entry
    {
        /// <summary>
        /// The Validity Of The Text.
        /// </summary>
        public bool IsValid
        {
            get
            {
                //Check For Text
                if (Text == null)
                {
                    return false;
                }

                //Check Regex
                return Regex.IsMatch(Text.Trim(), RegexPattern);
            }
        }
        private readonly string RegexPattern = "";

        /// <summary>
        /// Fired When The Entry Has Finished Validating The Email.
        /// </summary>
        public event EventHandler<ValidationEventArgs> FinishedValidation;
        public static readonly BindableProperty FinishedValidationProp = BindableProperty.Create("Finished Validation", typeof(ValidationEventArgs), typeof(EventHandler<ValidationEventArgs>));

        public ValidationEntry(string Pattern)
        {
            //Save Pattern
            RegexPattern = Pattern;
            
            //Add Event To Validate The Number
            Unfocused += ValidationEntry_Unfocused;

            //Set Text Color
            BackgroundColor = Color.Transparent;
        }
        private void ValidationEntry_Unfocused(object sender, FocusEventArgs e)
        {
            //Fire Event
            FinishedValidation?.Invoke(this, new ValidationEventArgs(Text, IsValid));
        }
    }
}
