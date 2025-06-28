using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using api.Dtos;

namespace api.Validations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class AtLeastOneContactRequired : ValidationAttribute
    {
        private readonly string[] _propertyNames;

        public AtLeastOneContactRequired(params string[] propertyNames)
        {
            _propertyNames = propertyNames;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;

            var type = value.GetType();
            foreach (var propName in _propertyNames)
            {
                var prop = type.GetProperty(propName);
                if (prop != null)
                {
                    var val = prop.GetValue(value);
                    if (val is string s && !string.IsNullOrWhiteSpace(s))
                        return ValidationResult.Success;

                    if (val != null) // можно расширить под другие типы
                        return ValidationResult.Success;
                }
            }

            return new ValidationResult($"At least one of the following fields must be provided: {string.Join(", ", _propertyNames)}");
        }
    }
}