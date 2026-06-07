using System.ComponentModel.DataAnnotations;

namespace Ex8.Models
{
    public class Movie
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Thông tin không được để trống")]
        [StringLength (100, ErrorMessage = "Độ dài tối đa là 100 ký tự")]
        [Display(Name = "Tên phim")]
        public string Title { get; set; }
        
        [Display(Name = "Năm phát hành")]
        [Required(ErrorMessage = "Thông tin không được để trống")]
        [Range(1900, 2026, ErrorMessage = "Năm phát hành phải nằm trong khoảng từ 1900 đến 2026")]
        public int PublishYear { get; set; }


        [StringLength (1000, ErrorMessage = "Độ dài tối đa là 1000 ký tự")]
        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Display(Name = "Quốc gia")]
        [Required(ErrorMessage = "Thông tin không được để trống")]
        public string National { get; set; }


    }
}