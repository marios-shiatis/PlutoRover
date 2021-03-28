using System.Collections.Generic;

namespace PlumGuide.PlutoRover.Web.Models
{
    public class Planet
    {
        public string Name { get; set; }
        public List<Obstacle> Obstacles { get; set; }
        public GridAreaSize GridAreaSize { get; set; }
    }
}
