using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class RoadPoint : Point
    {
        public Guid RoadId { get; set; }
        public Guid? ParentPointId { get; set; }
        public RoadPointType Type { get; set; }
        public RoadPoint? ParentPoint { get; set; }
    }
}