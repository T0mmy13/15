using System.Globalization;
using System.Windows.Controls;

namespace EFCORE15.Validators
{
    public class PriceOtDo : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var input = (value ?? "").ToString().Trim();

     
            bool isNumber = decimal.TryParse(input.Replace('.', ','), NumberStyles.Any, new CultureInfo("ru-RU"), out decimal price);

            if (!isNumber)
            {
               
                return new ValidationResult(false, "Введите корректное число!");
            }

            if (price < 0)
            {
                return new ValidationResult(false, "Цена не может быть отрицательной");
            }

            return ValidationResult.ValidResult;
        }
    }
}