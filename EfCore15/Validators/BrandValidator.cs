using System.Globalization;
using System.Windows.Controls;

namespace EFCORE15.Validators
{
    public class BrandValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var input = (value ?? "").ToString().Trim();

            if (input == string.Empty)
                return new ValidationResult(false, "Ввод поля обязателен");

            if (input.Length > 50)
                return new ValidationResult(false, "Слишком большое название");

            return ValidationResult.ValidResult;
        }
    }
}
