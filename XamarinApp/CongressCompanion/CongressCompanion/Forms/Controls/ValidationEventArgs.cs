using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AE_Xamarin.Forms.Controls
{
    public class ValidationEventArgs
    {
        /// <summary>
        /// The Text That Is Being Tested.
        /// </summary>
        public string TextValue { get; private set; }

        /// <summary>
        /// The Bool Valid Text.
        /// </summary>
        public bool Valid { get; protected set; }

        public ValidationEventArgs(string Text, bool ValidInfo)
        {
            TextValue = Text;
            Valid = ValidInfo;
        }
    }
}
