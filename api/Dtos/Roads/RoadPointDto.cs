using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using api.Models;

namespace api.Dtos.Roads
{
    public class RoadPointDto
    {
        public Guid Id { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public RoadPointType Type { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid? ParentPointId { get; set; }
    }
}