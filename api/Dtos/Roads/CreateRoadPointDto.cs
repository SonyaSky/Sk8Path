using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using api.Models;

namespace api.Dtos.Roads
{
    public class CreateRoadPointDto
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public RoadPointType Type { get; set; }
    }
}