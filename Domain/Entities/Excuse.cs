namespace TheGreatExcusesApplication.Domain.Entities;

public class Excuse
{
    // Unique identifier for the Excuse
    public int Id { get; set; }
    
    // The text of the excuse
    public string Text { get; set; }

    // Category for the excuse ("Personal", "Work", etc.)
    public ExcuseCategory Category { get; set; }
    
    // Date when the excuse was created
    public DateTime CreatedDate { get; set; }
}