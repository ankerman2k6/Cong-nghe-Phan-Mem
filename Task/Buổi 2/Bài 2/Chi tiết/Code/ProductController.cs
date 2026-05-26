using Microsoft.AspNetCore.Mvc;

namespace StudentManager.Controllers;

public class ProductController : Controller
{
    // Action Detail: Nhận tham số id từ URL (route parameter)
    // Ví dụ: /Product/Detail/5 → id = 5
    public IActionResult Detail(int? id)
    {
        // Nếu không truyền id → hiển thị thông báo lỗi
        if (id == null)
        {
            ViewBag.Message = "Vui lòng truyền Product ID! Ví dụ: /Product/Detail/5";
            ViewBag.IsError = true;
        }
        else
        {
            ViewBag.Message = $"Product ID = {id}";
            ViewBag.IsError = false;
        }

        return View();
    }

    // Action Category: Nhận tham số name từ query string
    // Ví dụ: /Product/Category?name=Laptop → name = "Laptop"
    public IActionResult Category(string name)
    {
        // Nếu không truyền name → hiển thị thông báo lỗi
        if (string.IsNullOrEmpty(name))
        {
            ViewBag.Message = "Vui lòng truyền Category name! Ví dụ: /Product/Category?name=Laptop";
            ViewBag.IsError = true;
        }
        else
        {
            ViewBag.Message = $"Category = {name}";
            ViewBag.IsError = false;
        }

        return View();
    }
}
