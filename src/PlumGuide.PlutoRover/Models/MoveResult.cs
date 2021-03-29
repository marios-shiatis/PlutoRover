using PlumGuide.PlutoRover.Web.Models;
using System.Collections.Generic;

namespace PlumGuide.PlutoRover.Web.Services
{
    public class MoveResult
    {

        public bool Succeeded { get; }
        public IEnumerable<string> Errors { get; }
        public RoverPosition RoverPosition { get; set; }
        public Obstacle Obstacle { get; set; }

        internal MoveResult(bool succeeded,
            RoverPosition roverPosition = null,
            IEnumerable<string> errors = null,
            Obstacle obstacle = null)
        {
            Errors = errors;
            Obstacle = obstacle;
            Succeeded = succeeded;
            RoverPosition = roverPosition;
        }

        public static MoveResult Success(RoverPosition roverPosition, Obstacle obstacle = null) => new MoveResult(succeeded: true, roverPosition: roverPosition, obstacle: obstacle);
        public static MoveResult Failure(IEnumerable<string> errors) => new MoveResult(errors: errors, succeeded: false);

    }
}