using AddresBookEL.AllEnums;
using AddresBookEL.IdentityEntities;
using AddressBookBL.EmailSenderBusiness;
using AddressBookPL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AddressBookPL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser>
             _userManager;
        private readonly RoleManager<AppRole>
            _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailSenderManager
                _emailSenderManager;

        public AccountController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager, IEmailSenderManager emailSenderManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _emailSenderManager = emailSenderManager;
        }

        //Register Login Logout

        [HttpGet]
        public IActionResult Register()
        {
            if (_signInManager.IsSignedIn(HttpContext.User))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var checkUserMail =
                    _userManager.FindByEmailAsync(model.Email).Result; //Async olan metodu senkron hale getirir
                if (checkUserMail!=null)
                {
                    ModelState.AddModelError("", "Bu email sistemde zaten kayıtlıdır!");
                    return View(model);
                }

                var checkUserName =
                    _userManager.FindByNameAsync(model.UserName).Result; //Async olan metodu senkron hale getirir
                if (checkUserName != null)
                {
                    ModelState.AddModelError("", "Bu kullanıcı adı sistemde zaten kayıtlıdır!");
                    return View(model);
                }
                AppUser user = new AppUser()
                {
                    UserName=model.UserName,
                    Email=model.Email,
                    IsDeleted=false,
                    CreatedDate=DateTime.Now,
                    Name=model.Name,
                    Surname=model.Surname,
                    EmailConfirmed=true // bir sonraki projede mail aktivasyon emaili göndeririz
                    ,
                    PhoneNumber=
                     model.PhoneNumber
                };
                var result =
                    _userManager.CreateAsync(user,model.Password).Result;
                if (result.Succeeded)
                {
                    //kullanıcı eklendiyse o kullanıcıya rol atması yapacağız
                    var roleResult =
                            _userManager.AddToRoleAsync(user, AllRoles.Member.ToString()).Result;
                    if (roleResult.Succeeded)
                    {
                        #region MailGonder
                        //Hoşgeldiniz emailini atacağız.
                        var emailMsg = new EmailMessage()
                        {
                            To = new string[] { user.Email },
                            Subject = "Wissen302-AddressBook - HOŞGELDİNİZ!",
                            Body = $"<html lang='tr'><head></head><body>" +
                            $"Merhaba Sayın {user.Name} {user.Surname}, <br/> Sisteme kaydınız gerçekleşmiştir.</body></html>"
                        };
                        _emailSenderManager.SendEmail(emailMsg); 
                        #endregion

                        return RedirectToAction("Login", "Account", new { username = model.UserName });
                    }
                    else
                    {
                        ModelState.AddModelError("", "Kullanıcı eklendi! Rolü eklenemedi! Sistem yöneticisine mail atınız/ bilgi veriniz!");
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Kullanıcı eklenmedi!");
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Beklenmedik bir hata oluştu! Tekrar deneyiniz");
                return View(model);
            }

        }

        [HttpGet]
        public IActionResult Login(string? username)
        {
            if (_signInManager.IsSignedIn(HttpContext.User))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var user = _userManager.FindByNameAsync(model.Username).Result;
                if (user==null)
                {
                    ModelState
                        .AddModelError("","Kullanıcı bulunamadı!");
                    return View(model);
                }
                var signInResult =
                    _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false).Result;
                if (signInResult.Succeeded)
                {
                    //giriş yaptı 
                    if (_userManager.IsInRoleAsync(user,AllRoles.Member.ToString()).Result)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else if (_userManager.IsInRoleAsync(user, AllRoles.Admin.ToString()).Result)
                    {
                        return RedirectToAction("Dashboard", "Admin");
                    }
                    return View(model);
                }
                else
                {
                    ModelState
                       .AddModelError("", "Kullanıcı adı ya da şifreniz hatalıdır!");
                    return View(model);
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Beklenmedik bir hata oluştu! Tekrar deneyiniz");
                return View(model);
            }
        }

        [Authorize]
        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
