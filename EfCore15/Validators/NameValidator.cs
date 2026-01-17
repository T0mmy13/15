using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EFCORE15.Validators
{
    public class NameValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var input = (value ?? "").ToString().Trim();

            if (input == string.Empty)
                return new ValidationResult(false, "Ввод поля обязателен");

            if (input.Length > 200)
                return new ValidationResult(false, "Слишком большое название");
            
            return ValidationResult.ValidResult;
        }
    }
}
