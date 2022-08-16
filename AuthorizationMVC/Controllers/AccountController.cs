using AuthorizationMVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthorizationMVC.Controllers
{
    public class AccountController : Controller
    {
        public List<UserModel> users = new List<UserModel>();

        public AccountController()
        {
            users.Add(new UserModel() { UserId = 1, Username = "ayd", Password = "123", Role = "Admin" });
            users.Add(new UserModel() { UserId = 1, Username = "asd", Password = "123", Role = "HR" });
            users.Add(new UserModel() { UserId = 2, Username = "other", Password = "123", Role = "Special" });
        }
        
        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel objLoginModel)
        {
            if (ModelState.IsValid)
            {
                var user = users.Where(x => x.Username == objLoginModel.UserName
                                        && x.Password == objLoginModel.Password).FirstOrDefault();
                if (user == null)
                {
                    ViewBag.Message = "Invalid Username or Password";
                    return View(objLoginModel);
                }
                else
                {
                    var claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier,Convert.ToString(user.UserId)),
                    new Claim(ClaimTypes.Name,user.Username),
                    new Claim(ClaimTypes.Role,user.Role),
                    new Claim("FavoriteDrink","Tea")
                    };
                   
                    var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                   
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);
                    
                    await HttpContext.SignInAsync("MyCookieAuth",
                        claimsPrincipal, new AuthenticationProperties() { IsPersistent = objLoginModel.RememberLogin });

                    return RedirectToAction("Index","Home");
                }
            }
            return View(objLoginModel);
        }

        public async Task<IActionResult> Logout()
        {
            //SignOutAsync is Extension method for SignOut
            await HttpContext.SignOutAsync("MyCookieAuth");
            //Redirect to home page
            return RedirectToAction("Index", "Home");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
