using PlumGuide.PlutoRover.Web.Exceptions;
using PlumGuide.PlutoRover.Web.Interface;
using PlumGuide.PlutoRover.Web.Models;
using System;
using System.Text.RegularExpressions;

namespace PlumGuide.PlutoRover.Web.Services
{
    public class NavigateService : INavigateService
    {
        private readonly Planet _planet;
        private RoverPosition _roverPosition;

        public NavigateService(Planet planet, RoverPosition roverPosition)
        {
            _planet = planet ?? throw new ArgumentNullException(nameof(planet));
            _roverPosition = roverPosition ?? throw new ArgumentNullException(nameof(roverPosition));
        }
        public MoveResult Move(NavigationCommand navigationCommand)
        {
            ValidateCommand(navigationCommand.Command);
            foreach (var command in navigationCommand.Command)
            {
                var tempRoverPosition = new RoverPosition()
                {
                    X = _roverPosition.X,
                    Y = _roverPosition.Y,
                    Direction = _roverPosition.Direction
                };

                switch (command)
                {
                    case 'F':
                        MoveForwards(tempRoverPosition);
                        break;
                    case 'B':
                        MoveBackwards(tempRoverPosition);
                        break;
                    case 'L':
                        TurnLeft(tempRoverPosition);
                        break;
                    case 'R':
                        TurnRight(tempRoverPosition);
                        break;
                    default:
                        break;
                }

                Guard.WrapEdges(_roverPosition, _planet);

                if (Guard.ObstacleDetected(tempRoverPosition, _planet))
                {
                    return MoveResult.Success(_roverPosition,
                        obstacle: new Obstacle { X = tempRoverPosition.X, Y = tempRoverPosition.Y });
                }
                else
                {
                    _roverPosition.X = tempRoverPosition.X;
                    _roverPosition.Y = tempRoverPosition.Y;
                    _roverPosition.Direction = tempRoverPosition.Direction;
                }
            }

            return MoveResult.Success(_roverPosition);
        }

        // This is a second level of check. Normally the error will yield earlier when called from the web API 
        // because we are using DataAnnotations in the NavigationCommand model.
        private void ValidateCommand(string command)
        {
            var r = new Regex(Constants.Command.ValidCommandRegex);
            if (!r.IsMatch(command))
            {
                throw new NavigationException($"Command {command} is not a known command. Please use only the following characters: 'F','B','L','R'");
            }
        }

        private void MoveForwards(RoverPosition position)
        {
            if (position.Direction == CompassDirections.North)
                position.Y += 1;
            if (position.Direction == CompassDirections.East)
                position.X += 1;
            if (position.Direction == CompassDirections.South)
                position.Y -= 1;
            if (position.Direction == CompassDirections.West)
                position.X -= 1;
        }

        private void MoveBackwards(RoverPosition position)
        {
            if (position.Direction == CompassDirections.North)
                position.Y -= 1;
            if (position.Direction == CompassDirections.East)
                position.X -= 1;
            if (position.Direction == CompassDirections.South)
                position.Y += 1;
            if (position.Direction == CompassDirections.West)
                position.X += 1;
        }

        private void TurnRight(RoverPosition position)
        {
            position.Direction =
                position.Direction + 1 > CompassDirections.West
                ? CompassDirections.North
                : position.Direction + 1;
        }

        private void TurnLeft(RoverPosition position)
        {
            position.Direction =
               position.Direction - 1 < CompassDirections.North
               ? CompassDirections.West
               : position.Direction - 1;
        }
    }
}
