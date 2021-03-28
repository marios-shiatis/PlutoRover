using PlumGuide.PlutoRover.Web.Models;
using System;

namespace PlumGuide.PlutoRover.Web.Services
{
    public static class Guard
    {
        public static void WrapEdges(RoverPosition roverPosition, Planet planet)
        {
            if (roverPosition.X > planet.GridAreaSize.X || roverPosition.X < 0)
            {
                roverPosition.X = WrapCoordinate(roverPosition.X, planet.GridAreaSize.X);
            }

            if (roverPosition.Y > planet.GridAreaSize.Y || roverPosition.Y < 0)
            {
                roverPosition.Y = WrapCoordinate(roverPosition.Y, planet.GridAreaSize.Y);
            }
        }

        private static int WrapCoordinate(int point, int maxSize)
        {
            return point > 0
                ? Math.Abs(point % maxSize)
                : maxSize - Math.Abs(point % maxSize);
        }
    }
}