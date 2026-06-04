using Microsoft.AspNetCore.Mvc;
using Ex7.Models;

namespace Ex7.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        //Action 1: Get URL /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        //Action 2: Post URL /Account/Login
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            //Kiểm tra dữ liệu hợp lệ
            if (ModelState.IsValid)
            {
                if(model.Username == "Antony" && model.Password == "123")
                {
                    //Lưu người dùng vào tempdata khi đăng nhập thành công
                    TempData["Username"] = model.Username;

                    //Redirect đến trang chủ sau khi đăng nhập thành công
                    return RedirectToAction("Dashboard");

                }
                else
                {
                    //Đăng nhập thát bại, hiển thị lỗi
                    ViewBag.ErrorMessage = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
                
            }
            return View(model);
        }
        //Action 3: Trang sau khi đăng nhập thành công
        public IActionResult Dashboard()
        {
            //Đọc tên người dùng từ tempdata
            ViewBag.Username = TempData["Username"];
            return View();
        }

        
    }
}