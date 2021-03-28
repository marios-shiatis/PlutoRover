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
        private readonly RoverPosition _roverPosition;

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
                switch (command)
                {
                    case 'F':
                        MoveForwards();
                        break;
                    case 'B':
                        MoveBackwards();
                        break;
                    case 'L':
                        TurnLeft();
                        break;
                    case 'R':
                        TurnRight();
                        break;
                    default:
                        break;
                }
                Guard.WrapEdges(_roverPosition, _planet);
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

        private void MoveForwards()
        {
            if (_roverPosition.Direction == CompassDirections.North)
                _roverPosition.Y += 1;
            if (_roverPosition.Direction == CompassDirections.East)
                _roverPosition.X += 1;
            if (_roverPosition.Direction == CompassDirections.South)
                _roverPosition.Y -= 1;
            if (_roverPosition.Direction == CompassDirections.West)
                _roverPosition.X -= 1;
        }

        private void MoveBackwards()
        {
            if (_roverPosition.Direction == CompassDirections.North)
                _roverPosition.Y -= 1;
            if (_roverPosition.Direction == CompassDirections.East)
                _roverPosition.X -= 1;
            if (_roverPosition.Direction == CompassDirections.South)
                _roverPosition.Y += 1;
            if (_roverPosition.Direction == CompassDirections.West)
                _roverPosition.X += 1;
        }

        private void TurnRight()
        {
            _roverPosition.Direction =
                _roverPosition.Direction + 1 > CompassDirections.West
                ? CompassDirections.North
                : _roverPosition.Direction + 1;
        }

        private void TurnLeft()
        {
            _roverPosition.Direction =
               _roverPosition.Direction - 1 < CompassDirections.North
               ? CompassDirections.West
               : _roverPosition.Direction - 1;
        }
    }
}
