using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Configuration
{
    public int Id { get; set; }

    [MaxLength(128)]
    public string Name { get; set; } = default!;

    public int BoardWidth { get; set; }
        
    public int BoardHeight { get; set; } 
        
    public int GridSize { get; set; }  

    public int WinCondition { get; set; } 

   
    public int MovePieceAfterNMoves { get; set; }
    

    // Collection of associated SaveGames
    public ICollection<SaveGame>? SaveGames { get; set; }
}