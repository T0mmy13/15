using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EFCORE15.Validators
{
    public class DescriptionValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var input = (value ?? "").ToString().Trim();

            if (input == string.Empty)
                return new ValidationResult(false, "Ввод поля обязателен");

            if (input.Length > 255)
                return new ValidationResult(false, "Слишком большое описание");

            return ValidationResult.ValidResult;
        }
    }
}
