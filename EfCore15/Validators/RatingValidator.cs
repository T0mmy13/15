using System.Globalization;
using System.Windows.Controls;

namespace EFCORE15.Validators
{
    public class RatingValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var input = (value ?? "").ToString().Trim();

            if (string.IsNullOrEmpty(input))
                return new ValidationResult(false, "Рейтинг обязателен");

            var normalizedInput = input.Replace(',', '.');

            if (!decimal.TryParse(normalizedInput, NumberStyles.Any, CultureInfo.InvariantCulture, out var rating))
            {
                return new ValidationResult(false, "Введите число (например 4.5)");
            }

            if (rating < 0.0m || rating > 5.0m)
            {
                return new ValidationResult(false, "Рейтинг должен быть от 0 до 5");
            }

            return ValidationResult.ValidResult;
        }
    }
}