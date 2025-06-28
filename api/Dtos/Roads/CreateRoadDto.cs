using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using api.Validations;

namespace api.Dtos.Roads
{
    public class CreateRoadDto
    {
        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }
        [Required]
        [MinCount(2, ErrorMessage = "At least 2 points are required.")]
        public List<CreateRoadPointDto> Points { get; set; } = new List<CreateRoadPointDto>();
    }
}