package org.example.studentmanagement.controller;


import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.ResponseBody;

@Controller
@RequestMapping("/home")
public class HomeController {
    @GetMapping("/index") //Xử lý đường dẫn home/index
    @ResponseBody
    public String index() {
        return "Welcome to Spring Boot!"; //Nội dung hiển thị
    }

    @GetMapping("/about")
    @ResponseBody
    public String about() {
        return "Họ và tên: Nguyễn Thành An";
    }

    @GetMapping("/contact")
    @ResponseBody
    public String contact() {
        return "Email: Bit240004";
    }





}
