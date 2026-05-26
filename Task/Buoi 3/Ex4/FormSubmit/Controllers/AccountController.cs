using Microsoft.AspNetCore.Mvc;
using FormSubmit.Models;

namespace FormSubmit.Controllers;

public class AccountController : Controller
{
    //Get: /Account/Login
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    //Post: /Account/Login
    [HttpPost]
    public IActionResult Login(LoginViewModel model)
    {
        if(model.Username =="admin" && model.Password == "123")
        {
            return View("LoginSuccess");
        }
        else
        {
            return View("LoginFailed");
        }
    }
}