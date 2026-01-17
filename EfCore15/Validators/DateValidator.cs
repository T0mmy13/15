using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EFCORE15.Validators
{
    public class DateValidator : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
                return new ValidationResult(false, "Дата обязательна");

            DateTime date;
            bool isValid = DateTime.TryParse(value.ToString(), out date);

            if (!isValid)
                return new ValidationResult(false, "Неверный формат даты");

            if (date > DateTime.Today)
                return new ValidationResult(false, "Дата не может быть позже сегодняшней");

            return ValidationResult.ValidResult;
        }
    }
}
