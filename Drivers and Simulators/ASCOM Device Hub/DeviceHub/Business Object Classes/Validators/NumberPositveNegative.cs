using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ASCOM.DeviceHub
{
    public class NumberPositiveNegative : ValidationRule
    {
        public override ValidationResult Validate
          (object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (value == null)
                return new ValidationResult(false, "value cannot be empty.");
            else
            {
                if (!Int32.TryParse(value.ToString(), out _))
                    return new ValidationResult(false, "Value is not a number.");
            }

            return ValidationResult.ValidResult;
        }
    }
}
