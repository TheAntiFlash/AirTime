using Model.Models;

namespace UserInterface.Data.Authentication;

public interface ILoginService
{
    Task Login(UserSession userSession);
    Task Logout();
}