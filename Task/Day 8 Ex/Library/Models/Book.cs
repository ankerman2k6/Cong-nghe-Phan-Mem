using System.ComponentModel.DataAnnotations;
namespace Library.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên sách không được để trống")]
        public string Name { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Giá sách phải lớn hơn 0")] //Quy tắc validation
        [Required(ErrorMessage = "Giá sách không được để trống")]
        public int Price { get; set; }

        public string imgLink {get; set;}

        


    }
}