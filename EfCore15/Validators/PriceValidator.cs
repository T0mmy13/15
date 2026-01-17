using System;
using System.Globalization;
using System.Windows.Controls;

namespace EFCORE15.Validators
{
    public class PriceValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var input = (value ?? "").ToString().Trim();

            if (string.IsNullOrEmpty(input))
                return new ValidationResult(false, "Цена обязательна");

            bool isNumber = decimal.TryParse(input.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var price)
                         || decimal.TryParse(input, NumberStyles.Any, CultureInfo.CurrentCulture, out price);

            if (!isNumber)
            {
                return new ValidationResult(false, "Введите корректное число");
            }

            if (price < 0)
            {
                return new ValidationResult(false, "Цена не может быть отрицательной");
            }

        
            if (price != Math.Round(price, 2))
            {
                return new ValidationResult(false, "Максимум 2 знака после запятой (например 10.99)");
            }

            if (price > 99999999999999.99m)
            {
                return new ValidationResult(false, "Слишком большая сумма");
            }

            return ValidationResult.ValidResult;
        }
    }
}