using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using api.Dtos.Roads;

namespace api.Validations
{
    public class MinCountAttribute : ValidationAttribute
    {
        private readonly int _min;

        public MinCountAttribute(int min)
        {
            _min = min;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is ICollection<CreateRoadPointDto> list && list.Count >= _min)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult($"At least {_min} point(s) are required.");
        }
    }
}