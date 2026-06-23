package org.example.studentmanagement.controller;


import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

@Controller
@RequestMapping("/product")
public class ProductController {
    //Lấy id từ đường dẫn /product/detail/{id}
    @GetMapping("detail/{id}")
    @ResponseBody
    public String getProductDetail(@PathVariable(value = "id", required = false) String id) {
        if(id==null){
            return "Lỗi! Dữ liệu không hợp lệ";
        }
        else return "Product id = " + id; //hiển thị kết quả
    }

    @GetMapping("/category")
    @ResponseBody
    public String getProductCategory(@RequestParam(value = "name", required = false) String name) {
        if(name==null){
            return "Lỗi dữ liệu không hợp lệ";
        }
        else return "Product name = " + name;
    }
}
