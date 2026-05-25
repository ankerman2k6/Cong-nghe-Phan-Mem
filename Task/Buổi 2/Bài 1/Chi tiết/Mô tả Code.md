**Promt đã dùng:**

“Hiện tại tôi đang có bài tập 1 như này, Tôi cần bạn giúp tôi và code, giải thích code của bạn sinh ở file nào ? Có tác dụng gì? File chứa phần code đó có công dụng gì ? ”

**AI đã hỗ trợ:**

Sinh ra các file code giúp chạy chương trình
Hướng dẫn luồng chạy file

**Đã chỉnh sửa:**

File _Layout.cshtml để thêm 2 phần điều hướng


**Kiểm chứng Code:**

Luồng chạy:

B1: Hiện ra giao diện trang web, khi ấn vào tag About ở trên giao diện, đó sẽ là file Layout.cshtml ở đó sẽ nhận thẻ a có controller là Home và có action là About.

B2: Routing nhận Request tách URL ra controller sẽ là Home và Action sẽ là About. Tìm class HomeController và tìm ra Action About trong đó.

B3: Chạy hàm About đó, tạo thuộc tính StudentName trong đó và gán dữ liệu cho nó. Hàm sẽ trả về View( ) tương ứng với file cshtml cùng tên là About

B4: Render ra giao diện của trang About đó và gọi ra thuộc tính StudentName ở trong đó
