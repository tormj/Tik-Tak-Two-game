using System.Text.Json;
using GameBrain;

public class TicTacTwoBrain
{
    private EGamePiece[][] _gameBoard;
    private int _gridSize;
    private GameConfiguration _gameConfiguration;
    private EGamePiece _nextMoveBy = EGamePiece.X;
    
    public string WinMessage { get; private set; }
    public string GameMode { get; set; } = "TwoPlayer";

    public int TotalMovesMade { get; private set; }

    // Grid Position, using X and Y coordinates
    public int GridPositionX { get; private set; }
    public int GridPositionY { get; private set; }

    public TicTacTwoBrain(GameConfiguration gameConfiguration)
    {
        _gameConfiguration = gameConfiguration;
        _gridSize = _gameConfiguration.GridSize;

        // Set initial grid position using GridPositionX and GridPositionY
        GridPositionX = (_gameConfiguration.BoardWidth - _gridSize) / 2;
        GridPositionY = (_gameConfiguration.BoardHeight - _gridSize) / 2;

        _gameBoard = new EGamePiece[_gameConfiguration.BoardWidth][];
        for (var x = 0; x < _gameBoard.Length; x++)
        {
            _gameBoard[x] = new EGamePiece[_gameConfiguration.BoardHeight];
        }
    }

    public int DimX => _gameBoard.Length;
    public int DimY => _gameBoard.Length > 0 ? _gameBoard[0].Length : 0;

    public EGamePiece[][] GameBoard => _gameBoard;
    public int GridSize => _gridSize;

    // Get the current player based on TotalMovesMade
    public EGamePiece GetCurrentPlayer()
    {
        return _nextMoveBy; // Return the current player (either X or O)
    }

    public bool MakeAMove(int x, int y)
    {
        if (_gameBoard[x][y] != EGamePiece.Empty) return false;

        _gameBoard[x][y] = GetCurrentPlayer();
        TotalMovesMade++;
        SwitchPlayer();
        return true;
    }

    // Check the win condition
    public EGamePiece? CheckWinCondition()
    {
        for (int x = GridPositionX; x < GridPositionX + GridSize; x++)
        {
            for (int y = GridPositionY; y < GridPositionY + GridSize; y++)
            {
                if (CheckLine(x, y, 1, 0) || // Check row
                    CheckLine(x, y, 0, 1) || // Check column
                    CheckLine(x, y, 1, 1) || // Check diagonal down-right
                    CheckLine(x, y, 1, -1)) // Check diagonal up-right
                {
                    WinMessage = $"Player {_nextMoveBy} wins!";
                    return _gameBoard[x][y];
                }
            }
        }

        return null;
    }

    private bool CheckLine(int startX, int startY, int stepX, int stepY)
    {
        var piece = _gameBoard[startX][startY];
        if (piece == EGamePiece.Empty) return false;

        for (int i = 1; i < _gameConfiguration.WinCondition; i++)
        {
            int newX = startX + i * stepX;
            int newY = startY + i * stepY;

            if (newX < 0 || newY < 0 || newX >= _gameConfiguration.BoardWidth || newY >= _gameConfiguration.BoardHeight)
                return false;

            if (_gameBoard[newX][newY] != piece)
                return false;
        }

        return true;
    }

    // Move the grid on the game board
    public bool MoveGrid(int deltaX, int deltaY)
    {
        int newGridX = GridPositionX + deltaX;
        int newGridY = GridPositionY + deltaY;

        // Check if the new grid position is out of bounds
        if (newGridX < 0 || newGridY < 0 ||
            newGridX + _gridSize > _gameConfiguration.BoardWidth ||
            newGridY + _gridSize > _gameConfiguration.BoardHeight)
        {
            Console.WriteLine("Invalid move! The grid cannot be moved out of bounds.");
            return false;
        }

        // Update the grid position
        GridPositionX = newGridX;
        GridPositionY = newGridY;

        Console.WriteLine($"Grid moved successfully to new position: ({GridPositionX}, {GridPositionY})");

        return true;
    }

    public void SetStartingPlayer(EGamePiece startingPlayer)
    {
        _nextMoveBy = startingPlayer;
    }

    public void LoadGameState(GameState gameState)
    {
        _gameConfiguration = gameState.GameConfiguration;
        _gameBoard = gameState.GameBoard;
        GridPositionX = gameState.GridPositionX;
        GridPositionY = gameState.GridPositionY;
        _nextMoveBy = gameState.NextMoveBy;
        TotalMovesMade = gameState.TotalMovesMade;
        _gameConfiguration = gameState.GameConfiguration;
        GameMode = gameState.GameMode;
    }

    public GameState GetGameState()
    {
        return new GameState(_gameBoard, _gameConfiguration)
        {
            NextMoveBy = _nextMoveBy,
            GridPositionX = GridPositionX,
            GridPositionY = GridPositionY,
            GameMode = GameMode
        };
    }

    public void SetWinCondition(int winCondition)
    {
        _gameConfiguration.WinCondition = winCondition;
    }

    public bool MovePiece(int fromX, int fromY, int toX, int toY)
    {
        // Validate the source position
        if (_gameBoard[fromX][fromY] == EGamePiece.Empty)
        {
            Console.WriteLine("Source position is empty.");
            return false;
        }

        // Validate the target position
        if (_gameBoard[toX][toY] != EGamePiece.Empty)
        {
            Console.WriteLine("Target position is not empty.");
            return false;
        }

        // Ensure the coordinates are within bounds
        if (toX < 0 || toX >= DimX || toY < 0 || toY >= DimY)
        {
            Console.WriteLine("Target position is out of bounds.");
            return false;
        }

        // Move the piece
        _gameBoard[toX][toY] = _gameBoard[fromX][fromY];
        _gameBoard[fromX][fromY] = EGamePiece.Empty;

        TotalMovesMade++;
        _nextMoveBy = _nextMoveBy == EGamePiece.X ? EGamePiece.O : EGamePiece.X;

        return true;
    }

    public void SwitchPlayer()
    {
        Console.WriteLine($"[DEBUG] Switching player from {_nextMoveBy}");
        _nextMoveBy = _nextMoveBy == EGamePiece.X ? EGamePiece.O : EGamePiece.X;
        Console.WriteLine($"[DEBUG] Next player is {_nextMoveBy}");
    }

    public bool MakeAiMove(EGamePiece piece)
    {
        // Check for an immediate win or block via piece placement
        if (MakeStrategicMove(piece))
        {
            Console.WriteLine("AI decided to place a piece.");
            return true; // Piece placed
        }

        // Check for grid movement opportunities
        if (TryMoveGrid(piece))
        {
            Console.WriteLine("AI decided to move the grid.");
            return true; // Grid moved
        }

        // Default to a random move if no other strategy applies
        return MakeRandomMove(piece);
    }

    private bool MakeStrategicMove(EGamePiece piece)
    {
        if (TryWinningMove(piece)) return true; // Try to win
        if (TryBlockingMove(piece)) return true; // Try to block the opponent
        return MakeRandomMove(piece); // Make a random move if no strategic options
    }

    private bool TryWinningMove(EGamePiece piece)
    {
        for (int x = 0; x < DimX; x++)
        {
            for (int y = 0; y < DimY; y++)
            {
                if (GameBoard[x][y] == EGamePiece.Empty)
                {
                    // Simulate the move
                    GameBoard[x][y] = piece;

                    // Check if this move results in a win
                    if (CheckWinCondition() == piece)
                    {
                        Console.WriteLine($"AI winning move at ({x}, {y})");
                        return true; // Winning move made
                    }

                    // Undo the move
                    GameBoard[x][y] = EGamePiece.Empty;
                }
            }
        }

        return false; // No winning move found
    }

    private bool TryBlockingMove(EGamePiece piece)
    {
        var opponent = piece == EGamePiece.X ? EGamePiece.O : EGamePiece.X;

        for (int x = 0; x < DimX; x++)
        {
            for (int y = 0; y < DimY; y++)
            {
                if (GameBoard[x][y] == EGamePiece.Empty)
                {
                    // Simulate the opponent's move
                    GameBoard[x][y] = opponent;

                    // Check if this move results in a win for the opponent
                    if (CheckWinCondition() == opponent)
                    {
                        // Block the opponent by placing the AI's piece
                        GameBoard[x][y] = piece;
                        Console.WriteLine($"AI blocked opponent at ({x}, {y})");
                        return true; // Blocking move made
                    }

                    // Undo the opponent's move
                    GameBoard[x][y] = EGamePiece.Empty;
                }
            }
        }

        return false; // No blocking move found
    }

    private bool MakeRandomMove(EGamePiece piece)
    {
        var emptyCells = new List<(int x, int y)>();

        for (int x = 0; x < DimX; x++)
        {
            for (int y = 0; y < DimY; y++)
            {
                if (GameBoard[x][y] == EGamePiece.Empty)
                {
                    emptyCells.Add((x, y));
                }
            }
        }

        if (emptyCells.Count > 0)
        {
            var random = new Random();
            var (x, y) = emptyCells[random.Next(emptyCells.Count)];
            GameBoard[x][y] = piece;
            Console.WriteLine($"AI placed randomly at ({x}, {y})");
            return true; // Random move made
        }

        return false; // No valid moves
    }

    private bool TryMoveGrid(EGamePiece piece)
    {
        var directions = new[] { (0, -1), (0, 1), (-1, 0), (1, 0) }; // Up, Down, Left, Right
        var opponent = piece == EGamePiece.X ? EGamePiece.O : EGamePiece.X;

        foreach (var (dx, dy) in directions)
        {
            // Save the current grid position
            int originalGridX = GridPositionX;
            int originalGridY = GridPositionY;

            // Simulate grid movement
            if (MoveGrid(dx, dy))
            {
                // Check if the grid movement creates a new winning opportunity for the AI
                if (CheckWinningOpportunities(piece))
                {
                    Console.WriteLine(
                        $"AI moved the grid to ({GridPositionX}, {GridPositionY}) for a winning opportunity.");
                    return true; // Keep the grid move
                }

                // Check if the grid movement blocks a winning opportunity for the opponent
                if (CheckWinningOpportunities(opponent))
                {
                    Console.WriteLine(
                        $"AI moved the grid to ({GridPositionX}, {GridPositionY}) to block the opponent.");
                    return true; // Keep the grid move
                }

                // Undo the grid movement if it doesn't help
                GridPositionX = originalGridX;
                GridPositionY = originalGridY;
            }
        }

        return false; // No beneficial grid movement found
    }

    private bool CheckWinningOpportunities(EGamePiece piece)
    {
        for (int x = GridPositionX; x < GridPositionX + GridSize; x++)
        {
            for (int y = GridPositionY; y < GridPositionY + GridSize; y++)
            {
                // Temporarily place the piece at (x, y) and check for a win
                if (GameBoard[x][y] == EGamePiece.Empty)
                {
                    GameBoard[x][y] = piece;
                    if (CheckWinCondition() == piece)
                    {
                        GameBoard[x][y] = EGamePiece.Empty; // Undo the temporary move
                        return true; // Found a winning opportunity
                    }

                    GameBoard[x][y] = EGamePiece.Empty; // Undo the temporary move
                }
            }
        }

        return false; // No winning opportunities found
    }

    public EGamePiece GetPieceAt(int x, int y)
    {
        if (x >= 0 && x < DimX && y >= 0 && y < DimY)
        {
            return _gameBoard[x][y]; // Return the piece at the specified coordinates
        }
        else
        {
            return EGamePiece.Empty; // Return Empty if the coordinates are out of bounds
        }

    }
}
