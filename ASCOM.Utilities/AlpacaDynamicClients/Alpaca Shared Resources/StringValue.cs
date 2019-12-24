using System.ComponentModel;

namespace ASCOM.Remote
{
    /// <summary>
    /// Class to hold a string value for use with the data grid view control. 
    /// This is required in order for the data grid view control to be able to bind to a List<string> variable
    /// String values are trimmed to ensure that searches for the "*" wild card character succeed even if the user pre or postpends spaces when entering the value.
    /// </summary>
    public class StringValue
    {
        string stringValue;

        // Initialise the string value, trimming it first
        public StringValue(string s)
        {
            stringValue = s.Trim();
        }

        [DisplayName("Permitted CORS Origins")]
        public string Value
        {
            get // Return the string value
            {
                return stringValue;
            }
            set // Set the string value, trimming it first
            {
                stringValue = value.Trim();
            }
        }
    }
}
