namespace TheGreatExcusesApplication.Domain.Entities;

public class Score
{
    // Unique identifier for the Score
    public int Id { get; set; }
    
    // Excuse that this Score is linked to
    public int ExcuseId { get; set; }
    
    // Value indicating how many times this excuse was successful 
    public int Value { get; set; }
}