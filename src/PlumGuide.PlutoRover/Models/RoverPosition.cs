using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;


namespace PlumGuide.PlutoRover.Web.Models
{
    public class RoverPosition : CartesianCoordinates
    {
        [EnumDataType(typeof(CompassDirections))]
        [JsonConverter(typeof(StringEnumConverter))]
        public CompassDirections Direction { get; set; }
    }
}
