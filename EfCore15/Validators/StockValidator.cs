using System.Globalization;
using System.Windows.Controls;

namespace EFCORE15.Validators
{
    public class StockValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var input = (value ?? "").ToString().Trim();

            if (string.IsNullOrEmpty(input))
                return new ValidationResult(false, "Ввод поля обязателен");

            if (!int.TryParse(input, out int stock))
            {
                return new ValidationResult(false, "Введите целое число!");
            }

            if (stock < 0)
            {
                return new ValidationResult(false, "Количество не может быть отрицательным");
            }

            return ValidationResult.ValidResult;
        }
    }
}