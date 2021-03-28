using PlumGuide.PlutoRover.Web.Services;

namespace PlumGuide.PlutoRover.Web.Interface
{
    public interface INavigateService
    {
        MoveResult Move(NavigationCommand command);
    }
}