using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Spots
{
    public class SpotDto
    {
        public Guid Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime CreateDate { get; set; }
        public string AuthorId { get; set; }
        public double Rating { get; set; }
    }
}