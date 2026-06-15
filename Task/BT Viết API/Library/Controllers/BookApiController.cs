using Microsoft.AspNetCore.Mvc;
using Library.Models;
using Library.Data;
using System.Linq;
namespace Library.Controllers
{
    //Cấu hình API

    [Route("api/book")]
    [ApiController]
    public class BookApiController : ControllerBase
    {
        private readonly LibraryContext _context;

        public BookApiController(LibraryContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult CreateBook([FromBody] Book book)
        {
            //Api sử lý validate
            _context.Books.Add(book);
            _context.SaveChanges();

            // Trả về mã 201 Created cùng với dữ liệu vừa được tạo
            return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
            
        }

        [HttpGet("{id}")]
        public IActionResult GetBookById(int id)
        {
            //Validate
            if(id <= 0)
            {
                return BadRequest(new { 
                    Error = "ID không hợp lệ", 
                    Message = "ID phải là một số nguyên dương." 
                });
            }

            var book =  _context.Books.FirstOrDefault(b => b.Id == id);

            if(book == null)
            {
                return NotFound(new {Message = $"Không tìm thấy sách có ID = {id}"});

            }
            return Ok(book);    
        }

    }
}