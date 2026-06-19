using Microsoft.EntityFrameworkCore;
namespace MidTermBIT240004.Data 
{
    public class ApplicationDbContext : DbContext
    {
        // Constructor để ASP.NET Core Dependency Injection cấu hình Chuỗi kết nối (Connection String)
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Khai báo các DbSet để làm việc với các bảng trong Database
        public DbSet<EventCategory_BIT240004> EventCategories_BIT240004 { get; set; }
        public DbSet<Event_BIT240004> Events_BIT240004 { get; set; }
        public DbSet<EventImage_BIT240004> EventImages_BIT240004 { get; set; }

        // Cấu hình Fluent API cho mối quan hệ dữ liệu và ràng buộc
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ==========================================
            // CHỨC NĂNG 1 & 5: QUAN HỆ CATEGORY - EVENT
            // ==========================================
            // Một Event thuộc về một EventCategory, một EventCategory có nhiều Event
            modelBuilder.Entity<Event_BIT240004>()
                .HasOne(e => e.EventCategory)
                .WithMany(c => c.Events)
                .HasForeignKey(e => e.EventCategoryId)
                // Chức năng 5: Không cho phép xóa loại sự kiện đang có sự kiện sử dụng
                // DeleteBehavior.Restrict sẽ ném ra ngoại lệ/lỗi nếu cố tình xóa một Category đang có Event liên kết
                .OnDelete(DeleteBehavior.Restrict); 

            // ==========================================
            // CHỨC NĂNG 1: QUAN HỆ EVENT - EVENTIMAGE
            // ==========================================
            // Một EventImage thuộc về một Event, một Event có nhiều EventImage
            modelBuilder.Entity<EventImage_BIT240004>()
                .HasOne(img => img.Event)
                .WithMany(e => e.EventImages)
                .HasForeignKey(img => img.EventId)
                // Ở đây dùng DeleteBehavior.Cascade: Khi xóa Event (đáp ứng đủ điều kiện không phải đang diễn ra),
                // thì toàn bộ ảnh thuộc Event đó trong bảng EventImages cũng sẽ tự động bị xóa theo để tránh rác dữ liệu.
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}