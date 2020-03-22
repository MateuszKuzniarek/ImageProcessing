using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ImageProcessing
{
    public class PositiveIntegerValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string strValue = Convert.ToString(value);

            if (string.IsNullOrEmpty(strValue))
            {
                return new ValidationResult(false, $"Incorrect input");
            }
                
            bool canConvert = false;
            canConvert = int.TryParse(strValue, out int intVal);
            if (canConvert && intVal > 0)
            {
                return new ValidationResult(true, null);
            }

            return new ValidationResult(false, $"Incorrect input");
        }
    }
}
