using AddresBookEL.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AddressBookPL.Components
{
    public class MenuViewComponent:ViewComponent
    {
        private readonly UserManager<AppUser>
            _userManager;
        public MenuViewComponent(UserManager<AppUser>
            userManager)
        {
            _userManager = userManager;
        }
       public IViewComponentResult Invoke()
        {
            string? userid = HttpContext.User.Identity?.Name;
            TempData["LoggedInUserNameSurname"] = null;
            if (userid != null)
            {
                var user = _userManager.FindByNameAsync(userid).Result;
                TempData["LoggedInUserNameSurname"] = $"{user.Name} {user.Surname}";
            }
            return View();
        }
    }
}
