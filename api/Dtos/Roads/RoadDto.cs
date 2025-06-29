using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Roads
{
    public class RoadDto
    {
        public Guid Id { get; set; }
        public string AuthorId { get; set; }
        public double Rating { get; set; }
        public Guid? FileId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<RoadPointDto> Points { get; set; } = new List<RoadPointDto>();
    }
}