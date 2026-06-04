namespace StudentManager.Controllers;

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using StudentManager.Models;

public class StudentController : Controller
{
    // Action Info: Hiển thị thông tin sinh viên
    public IActionResult Info()
    {
        ViewBag.StudentName = "Nguyễn Thành An";
        ViewData["StudentAge"] = 20;
        string major = "Công nghệ thông tin";
        return View((object)major);
    }
}