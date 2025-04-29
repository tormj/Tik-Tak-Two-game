using System.ComponentModel.DataAnnotations;

namespace Domain;

public class SaveGame
{
    public int Id { get; set; }

    [MaxLength(128)]
    public string CreatedAtDateTime { get; set; } = DateTime.Now.ToString("O");

    [MaxLength(10240)]

    // Foreign Key to Configuration
    public int ConfigurationId { get; set; }
    public Configuration? Configuration { get; set; }

    // New columns for Grid Position and Game Board
    public int GridPositionX { get; set; } // To store the X coordinate of the grid
    public int GridPositionY { get; set; } // To store the Y coordinate of the grid

    [MaxLength(10240)] // Assuming the GameBoard will be serialized as a string or JSON
    public string GameBoard { get; set; } = default!; // To store the game board state as string or JSON representation
    
    public int TotalMovesMade { get; set; }
    
    public int NextMoveBy { get; set; } 
    
    public string GameMode { get; set; }
}