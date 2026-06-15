using Microsoft.AspNetCore.Mvc;
using Library.Models;
using Library.Data;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;
using System.Threading.Tasks;
using System.Linq;


namespace Library.Controllers
{
    public class BookController : Controller
    {

        private readonly LibraryContext _context;

        private readonly IWebHostEnvironment _webHostEnvironment;

        //Inject LibraryContext vào controller thông qua constructor
        public BookController(LibraryContext context,IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;

        }
        public IActionResult Index()
        {
            var books = _context.Books.ToList(); //Lấy danh sách sách từ database từ bảng Books trong LibraryContext
            return View(books); //Truyền danh sách sách sang View 
        }

        public IActionResult Detail(int id) //nhận id của sách cần xem chi tiết từ URL
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id); //Tìm sách theo id
            if (book == null)
            {
                return NotFound(); //Nếu không tìm thấy sách, trả về lỗi 404
            }
            return View(book); //Truyền thông tin sách sang View
        }

        public IActionResult Create()
        {
            return View(); //Tìm đến Create.cshtml để chạy
        }
        // 3.  HÀM  ASYNC VÀ NHẬN THÊM THAM SỐ imageFile
        // POST: Book/Create
       [HttpPost]
        public async Task<IActionResult> Create(Book book, IFormFile imageFile) 
        {
            ModelState.Remove("imgLink");
            if (!ModelState.IsValid)
            {
                // Nếu có lỗi validation, trả về lại view với dữ liệu đã nhập
                return View(book); 
            }

            // --- BẮT ĐẦU PHẦN XỬ LÝ UPLOAD ẢNH ---
            if (imageFile != null && imageFile.Length > 0)
            {
                // Điều kiện: Chỉ nhận jpg/png
                var extension = Path.GetExtension(imageFile.FileName).ToLower();
                if (extension != ".jpg" && extension != ".png")
                {
                    ModelState.AddModelError("ImageError", "Chỉ cho phép upload file định dạng .jpg hoặc .png");
                    return View(book); // Báo lỗi nếu sai file
                }

                // Lưu ảnh lên server (thư mục wwwroot/images/books)
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "books");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Tạo tên file ngẫu nhiên để không bị trùng
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Copy file vào server
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                // Lưu đường dẫn vào trường imgLink của Model Book
                // Đảm bảo model Book của bạn có property tên là imgLink (hoặc ImgLink tùy cách bạn viết hoa thường)
                book.imgLink = "/images/books/" + uniqueFileName; 
            }
            // --- KẾT THÚC PHẦN XỬ LÝ UPLOAD ẢNH ---

            _context.Books.Add(book); // Thêm sách mới vào DbContext
            await _context.SaveChangesAsync(); // Lưu thay đổi vào database (Đổi sang dùng Async)
            
            return RedirectToAction("Index"); // Chuyển hướng về trang danh sách sách sau khi tạo thành công
        }

        //Sửa
        public IActionResult Edit(int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id); //Tìm sách theo id
            if (book == null)
            {
                return NotFound(); //Nếu không tìm thấy sách, trả về lỗi 404
            }
            return View(book); //Truyền thông tin sách sang View để hiển thị form sửa
        }

        [HttpPost]
        public IActionResult Edit(Book book)
        {
            if (!ModelState.IsValid)
            {
                //Nếu có lỗi validation, trả về lại view với dữ liệu đã nhập để hiển thị lỗi
                return View(book); //giữ lại phần dữ liệu đã nhập để hiển thị lỗi
            }
            var existingBook = _context.Books.FirstOrDefault(b => b.Id == book.Id); //Tìm sách theo id
            if (existingBook == null)
            {
                return NotFound(); //Nếu không tìm thấy sách, trả về lỗi 404
            }
            existingBook.Name = book.Name; //Cập nhật tên sách
            existingBook.Price = book.Price; //Cập nhật giá sách
            _context.SaveChanges(); //Lưu thay đổi vào database
            return RedirectToAction("Index");
        } //Chuyển hướng về trang danh sách sách sau

        //Xóa
        public IActionResult Delete(int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id); //Tìm sách theo id
            if (book == null)
            {
                return NotFound(); //Nếu không tìm thấy sách, trả về lỗi 404
            }
            return View(book); //Truyền thông tin sách sang View để hiển thị form xác nhận xóa
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id); //Tìm sách theo id
            if (book == null)
            {
                return NotFound(); //Nếu không tìm thấy sách, trả về lỗi 404
            }
            _context.Books.Remove(book); //Xóa sách khỏi DbContext
            _context.SaveChanges(); //Lưu thay đổi vào database
            return RedirectToAction("Index"); //Chuyển hướng về trang danh sách sách sau khi xóa thành công
        }
    }
}