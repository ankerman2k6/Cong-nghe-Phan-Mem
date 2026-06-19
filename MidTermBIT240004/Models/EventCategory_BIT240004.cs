using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("EventCategories_BIT240004")]
public class EventCategory_BIT240004
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(255)]
    public string Name { get; set; }
    
    public string Description { get; set; }

    // Relationship
    public ICollection<Event_BIT240004> Events { get; set; }
}