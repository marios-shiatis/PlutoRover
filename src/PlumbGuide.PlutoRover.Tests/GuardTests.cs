using FluentAssertions;
using PlumGuide.PlutoRover.Web.Models;
using PlumGuide.PlutoRover.Web.Services;
using Xunit;

namespace PlumbGuide.PlutoRover.Tests
{
    public class GuardTests
    {
        [Theory]
        [InlineData(110, 100, 10, 100)]
        [InlineData(910, 100, 10, 100)]

        [InlineData(-110, 100, 90, 100)]
        [InlineData(-910, 100, 90, 100)]
        [InlineData(-30, 100, 70, 100)]
        [InlineData(-330, 100, 70, 100)]

        [InlineData(110, 110, 10, 10)]
        [InlineData(910, 910, 10, 10)]

        [InlineData(-110, -100, 90, 100)]
        [InlineData(-910, -100, 90, 100)]

        [InlineData(-110, -110, 90, 90)]
        [InlineData(-910, -910, 90, 90)]
        [InlineData(-30, -30, 70, 70)]
        [InlineData(-330, -330, 70, 70)]
        public void OnNavigatingOutOfBoundaries_WrappingWorksAsExpected(int xPosition, int yPosition, int expectedXPosition, int expectedYPosition)
        {
            //Arrange
            var planet = new Planet()
            {
                GridAreaSize = new GridAreaSize() { X = 100, Y = 100 },
                Name = "Pluto",
                Obstacles = null
            };

            RoverPosition roverPosition = new RoverPosition() { X = xPosition, Y = yPosition };

            //Act
            Guard.WrapEdges(roverPosition, planet);
            
            //Assert
            roverPosition.X.Should().Be(expectedXPosition);
            roverPosition.Y.Should().Be(expectedYPosition);
        }
    }
}
