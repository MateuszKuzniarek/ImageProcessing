using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ImageProcessing
{
    public class MaskSizeValidationRule : ValidationRule
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
            if (canConvert && intVal > 0 && intVal % 2 == 1)
            {
                return new ValidationResult(true, null);
            }

            return new ValidationResult(false, $"Incorrect input");
        }
    }
}
