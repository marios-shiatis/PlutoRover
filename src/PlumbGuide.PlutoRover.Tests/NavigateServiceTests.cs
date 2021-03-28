using FluentAssertions;
using PlumGuide.PlutoRover.Web.Exceptions;
using PlumGuide.PlutoRover.Web.Models;
using PlumGuide.PlutoRover.Web.Services;
using System;
using System.Collections.Generic;
using Xunit;

namespace PlumbGuide.PlutoRover.Tests
{
    public class NavigateServiceTests
    {
        private RoverPosition _roverPosition;
        private Planet _planet;
        private NavigateService _sut;
        private List<Obstacle> _obstacles;

        [Theory]
        [InlineData("R", CompassDirections.North, CompassDirections.East)]
        [InlineData("R", CompassDirections.East, CompassDirections.South)]
        [InlineData("R", CompassDirections.South, CompassDirections.West)]
        [InlineData("R", CompassDirections.West, CompassDirections.North)]

        [InlineData("L", CompassDirections.North, CompassDirections.West)]
        [InlineData("L", CompassDirections.East, CompassDirections.North)]
        [InlineData("L", CompassDirections.South, CompassDirections.East)]
        [InlineData("L", CompassDirections.West, CompassDirections.South)]

        [InlineData("LR", CompassDirections.North, CompassDirections.North)]
        [InlineData("LR", CompassDirections.East, CompassDirections.East)]
        [InlineData("LR", CompassDirections.South, CompassDirections.South)]
        [InlineData("LR", CompassDirections.West, CompassDirections.West)]

        [InlineData("RL", CompassDirections.North, CompassDirections.North)]
        [InlineData("RL", CompassDirections.East, CompassDirections.East)]
        [InlineData("RL", CompassDirections.South, CompassDirections.South)]
        [InlineData("RL", CompassDirections.West, CompassDirections.West)]

        [InlineData("RRRR", CompassDirections.North, CompassDirections.North)]
        [InlineData("RRRR", CompassDirections.East, CompassDirections.East)]
        [InlineData("RRRR", CompassDirections.South, CompassDirections.South)]
        [InlineData("RRRR", CompassDirections.West, CompassDirections.West)]

        [InlineData("LLLL", CompassDirections.North, CompassDirections.North)]
        [InlineData("LLLL", CompassDirections.East, CompassDirections.East)]
        [InlineData("LLLL", CompassDirections.South, CompassDirections.South)]
        [InlineData("LLLL", CompassDirections.West, CompassDirections.West)]

        [InlineData("LLLLL", CompassDirections.North, CompassDirections.West)]
        [InlineData("LLLLL", CompassDirections.East, CompassDirections.North)]
        [InlineData("LLLLL", CompassDirections.South, CompassDirections.East)]
        [InlineData("LLLLL", CompassDirections.West, CompassDirections.South)]

        [InlineData("RRRRR", CompassDirections.North, CompassDirections.East)]
        [InlineData("RRRRR", CompassDirections.East, CompassDirections.South)]
        [InlineData("RRRRR", CompassDirections.South, CompassDirections.West)]
        [InlineData("RRRRR", CompassDirections.West, CompassDirections.North)]
        public void OnTurningDirection_RoverTurnsToTheExpectedCompassDirection(string command, CompassDirections currentCompassDirection, CompassDirections expectedDirection)
        {
            //Arrange
            InitialiseRoverLandingVariables(currentCompassDirection: currentCompassDirection);
            _sut = new NavigateService(_planet, _roverPosition);

            //Act
            var navigationCommand = new NavigationCommand() { Command = command };
            var result = _sut.Move(navigationCommand);

            //Assert
            result.Should().BeEquivalentTo(MoveResult.Success(_roverPosition));
            _roverPosition.Direction.Should().Be(expectedDirection);
        }

        [Theory]
        [InlineData("FFFF", CompassDirections.North, 0, 4, CompassDirections.North)]
        [InlineData("FFRFF", CompassDirections.North, 2, 2, CompassDirections.East)]
        [InlineData("FFFFRFFFF", CompassDirections.North, 4, 4, CompassDirections.East)]
        [InlineData("FFFFRFFFFRRFFFF", CompassDirections.North, 0, 4, CompassDirections.West)]
        public void OnReceivingValidCommands_RoverShouldEndUpFacingInTheRightDirectionWithTheExpectedCartesianCoordinates(string command, CompassDirections currentCompassDirection, int expectedXCoordinate, int expectedYCoordinate, CompassDirections expectedDirection)
        {
            //Arrange
            InitialiseRoverLandingVariables(currentCompassDirection: currentCompassDirection);
            _sut = new NavigateService(_planet, _roverPosition);

            //Act
            var navigationCommand = new NavigationCommand() { Command = command };
            var result = _sut.Move(navigationCommand);

            //Assert
            result.Should().BeEquivalentTo(MoveResult.Success(_roverPosition));
            _roverPosition.X.Should().Be(expectedXCoordinate);
            _roverPosition.Y.Should().Be(expectedYCoordinate);
            _roverPosition.Direction.Should().Be(expectedDirection);
        }

        [Theory]
        [InlineData("")]
        [InlineData("l")]
        [InlineData("r")]
        [InlineData("RD")]
        [InlineData("LN")]
        [InlineData("DS")]
        [InlineData("ABC")]
        [InlineData("LLFFO")]
        [InlineData("llrrl")]
        public void OnReceivingInvalidCommands_RoverShouldThrowException(string command)
        {
            //Arrange
            InitialiseRoverLandingVariables();
            _sut = new NavigateService(_planet, _roverPosition);

            //Act
            var navigationCommand = new NavigationCommand() { Command = command };
            Action act = () => _sut.Move(navigationCommand);

            //Assert
            act.Should()
                .Throw<NavigationException>()
                .WithMessage($"Command {command} is not a known command. Please use only the following characters: 'F','B','L','R'");
        }

        private void InitialiseRoverLandingVariables(List<Obstacle> obstacles = null, CompassDirections? currentCompassDirection = null)
        {
            _obstacles = obstacles ?? new List<Obstacle>() {
                new Obstacle(){X = 10, Y = 24 },
                new Obstacle(){X = 33, Y = 0 },
            };

            _roverPosition = new RoverPosition() { X = 0, Y = 0, Direction = currentCompassDirection ?? CompassDirections.North };

            _planet = new Planet()
            {
                Name = "Pluto",
                Obstacles = _obstacles,
                GridAreaSize = new GridAreaSize() { X = 100, Y = 100 }
            };
        }
    }
}
