using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("EventImages_BIT240004")]
public class EventImage_BIT240004
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string ImageUrl { get; set; }
    
    public bool IsThumbnail { get; set; }
    
    // Foreign Key
    public int EventId { get; set; }
    [ForeignKey("EventId")]
    public Event_BIT240004 Event { get; set; }
}