using System.Text.Json;
using GameBrain;

public class GameState
{
    public EGamePiece[][] GameBoard { get; set; }
    public GameConfiguration GameConfiguration { get; set; }
    public EGamePiece NextMoveBy { get; set; } = EGamePiece.X;
    
    

    // Grid Position X and Y for database storage
    public int GridPositionX { get; set; }
    public int GridPositionY { get; set; }
    
    public (int X, int Y) GridPosition
    {
        get => (GridPositionX, GridPositionY);
        set
        {
            GridPositionX = value.X;
            GridPositionY = value.Y;
        }
    }

    public int TotalMovesMade { get; set; }
    public string GameMode { get; set; }

    public GameState()
    {
    }

    public GameState(EGamePiece[][] board, GameConfiguration config)
    {
        GameBoard = board;
        GameConfiguration = config;
        GridPositionX = 0;  // Default position
        GridPositionY = 0;  // Default position
        TotalMovesMade = 0;
        GameMode = "";
    }

    public GameState(EGamePiece[][] board, GameConfiguration config, (int X, int Y) gridPosition)
    {
        GameBoard = board;
        GameConfiguration = config;
        GridPositionX = gridPosition.X;
        GridPositionY = gridPosition.Y;
        TotalMovesMade = 0;
        GameMode = "";
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}