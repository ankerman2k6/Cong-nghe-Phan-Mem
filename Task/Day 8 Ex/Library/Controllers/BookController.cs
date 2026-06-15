using Microsoft.AspNetCore.Mvc;
using Library.Models;
using Library.Data;


namespace Library.Controllers
{
    public class BookController : Controller
    {

        private readonly LibraryContext _context;

        //Inject LibraryContext vào controller thông qua constructor
        public BookController(LibraryContext context)
        {
            _context = context;
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
        //POST: Book/Create
        [HttpPost]
        public IActionResult Create(Book book) //Nhận dữ liệu sách mới từ form được submit trong Create.cshtml thông qua model binding
        {
            if (!ModelState.IsValid)
            {
                //Nếu có lỗi validation, trả về lại view với dữ liệu đã nhập để hiển thị lỗi
                return View(book); //giữ lại phần dữ liệu đã nhập để hiển thị lỗi
            }
            _context.Books.Add(book); //Thêm sách mới vào DbContext
            _context.SaveChanges(); //Lưu thay đổi vào database
            return RedirectToAction("Index"); //Chuyển hướng về trang danh sách sách sau khi tạo thành công
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