using GameBrain;

namespace ConsoleUI
{
    public static class Visualizer
    {
   public static void DrawBoard(TicTacTwoBrain gameInstance)
{
    Console.WriteLine($"Drawing board with grid size {gameInstance.GridSize} at position ({gameInstance.GridPositionX}, {gameInstance.GridPositionY})");

    // Draw the grid's top row (X-axis)
    Console.Write("    "); // Space for Y axis numbers
    for (var x = 0; x < gameInstance.DimX; x++)
    {
        Console.Write($"  {x} ");  // Print the X-axis numbers (0 to DimX-1)
    }
    Console.WriteLine();

    // Draw the top border of the grid
    Console.Write("   ┌");
    for (var x = 0; x < gameInstance.DimX - 1; x++)
    {
        Console.Write("───┬");
    }
    Console.WriteLine("───┐");

    // Loop through each row (Y axis)
    for (var y = 0; y < gameInstance.DimY; y++)
    {
        // Print Y-axis numbers (row labels)
        Console.Write($" {y} │");

        // Loop through each column in the row
        for (var x = 0; x < gameInstance.DimX; x++)
        {
            // Check if the cell is within the active grid area (where pieces are placed)
            bool isInActiveArea = x >= gameInstance.GridPositionX && x < gameInstance.GridPositionX + gameInstance.GridSize &&
                                  y >= gameInstance.GridPositionY && y < gameInstance.GridPositionY + gameInstance.GridSize;

            // Draw the piece at (x, y) position
            string piece = DrawGamePiece(gameInstance.GetPieceAt(x, y));
            Console.Write($" {piece} ");

            // Draw column separator if not the last column
            if (x < gameInstance.DimX - 1)
            {
                // Color the grid line (column separator)
                if (isInActiveArea)
                {
                    Console.ForegroundColor = ConsoleColor.Green;  // Active grid separator color
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Gray;  // Default color for non-active separators
                }
                Console.Write("│");
                Console.ResetColor();
            }
        }
        Console.WriteLine();

        // If it's not the last row, draw the row separator with colored grid lines
        if (y < gameInstance.DimY - 1)
        {
            Console.Write("   ├");
            for (var x = 0; x < gameInstance.DimX - 1; x++)
            {
                // Color the grid line (row separator)
                if (x >= gameInstance.GridPositionX && x < gameInstance.GridPositionX + gameInstance.GridSize &&
                    y >= gameInstance.GridPositionY && y < gameInstance.GridPositionY + gameInstance.GridSize)
                {
                    Console.ForegroundColor = ConsoleColor.Green;  // Active grid separator color
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Gray;  // Default color for non-active separators
                }
                Console.Write("───┼");
            }
            Console.WriteLine("───┤");
            Console.ResetColor();
        }
    }

    // Bottom border for the board
    Console.Write("   └");
    for (var x = 0; x < gameInstance.DimX - 1; x++)
    {
        // Color the bottom border line
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write("───┴");
    }
    Console.WriteLine("───┘");

    Console.ResetColor(); // Reset console color after drawing
}


        private static string DrawGamePiece(EGamePiece piece)
        {
            switch (piece)
            {
                case EGamePiece.O:
                    Console.ForegroundColor = ConsoleColor.Blue; 
                    return "O";
                case EGamePiece.X:
                    Console.ForegroundColor = ConsoleColor.Red;  
                    return "X";
                default:
                    Console.ResetColor(); 
                    return " "; // Empty space
            }
        }
    }
}
