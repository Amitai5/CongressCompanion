using System;

namespace AE_Xamarin.Managers
{
    public class SaveItemAttribute : Attribute
    {
        //Store The Default Value Of The Item
        public object DefaultValue { get; private set; }

        /// <summary>
        /// Creates The Save Item Attribute Which Marks This Object For Saving.
        /// </summary>
        /// <param name="Default">The Default Value Of The Object Once Reset.</param>
        public SaveItemAttribute(object Default)
        {
            DefaultValue = Default;
        }
    }
}