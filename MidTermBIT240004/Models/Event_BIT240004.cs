using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Events_BIT240004")]
public class Event_BIT240004
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(255)]
    public string Name { get; set; }
    
    public decimal Price { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    [StringLength(255)]
    public string Location { get; set; }
    
    public string Description { get; set; }
    
    // Foreign Key
    public int EventCategoryId { get; set; }
    [ForeignKey("EventCategoryId")]
    public EventCategory_BIT240004 EventCategory { get; set; }

    // Relationship
    public ICollection<EventImage_BIT240004> EventImages { get; set; }
}