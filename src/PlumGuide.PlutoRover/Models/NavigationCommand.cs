using System.ComponentModel.DataAnnotations;

namespace PlumGuide.PlutoRover.Web.Services
{
    public class NavigationCommand
    {
        [RegularExpression(
            pattern: Constants.Command.ValidCommandRegex,
            ErrorMessage = "The navigation command must contain only 'FBLR' characters.")]
        public string Command { get; set; }
    }
}