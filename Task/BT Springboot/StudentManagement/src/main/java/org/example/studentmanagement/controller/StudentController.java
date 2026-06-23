package org.example.studentmanagement.controller;

import org.example.studentmanagement.model.Student;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;

@Controller
@RequestMapping("/student")
public class StudentController {
    @GetMapping("/info")
    public String showStudentInfo(Model model) {
        //Khowir tạo dữ liệu
        Student student = new Student("Nguyễn Thành An", 20, "CNTT");
        model.addAttribute("student", student);
        //Trả về html tự tìm file info.html
        return "student/info";
    }
}
