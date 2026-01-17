using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EFCORE15.Validators

{
    public class c : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var input = (value ?? "").ToString().Trim();

            if (Convert.ToDecimal(input) < 0)
                return new ValidationResult(false, "Цена не может быть отрицательна");

            foreach (char c in input)
            {
                if (!char.IsDigit(c) && c != '.' && c != ',')
                    return new ValidationResult(false, "Введите число!");
            }

            return ValidationResult.ValidResult;
        }
    }
}
