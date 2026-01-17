using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EFCORE15.Validators
{
    public class ComboBoxValidator : ValidationRule
    {
        public string ErrorMessage { get; set; } = "Поле обязательно для заполнения";

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
                return new ValidationResult(false, "Поле обязательно для выбора");

            if (value is int id && id <= 0)
                return new ValidationResult(false, "Поле обязательно для выбора");

            return ValidationResult.ValidResult;
        }
    }
}
