using Domain;

namespace GameBrain
{
    public record struct GameConfiguration()
    {
        public int Id { get; set; } 
        public string Name { get; set; } = default!;
        public int BoardWidth { get; set; } = 5;
        public int BoardHeight { get; set; } = 5;
        public int WinCondition { get; set; } = 3;
        public int GridSize { get; set; } = 3;
        public int MovePieceAfterNMoves { get; set; } = 0;
        
        
        
        
        public static GameConfiguration FromDbConfig(Configuration config) =>
            new GameConfiguration
            {
                Id = config.Id,
                Name = config.Name,
                BoardWidth = config.BoardWidth,
                BoardHeight = config.BoardHeight,
                GridSize = config.GridSize,
                WinCondition = config.WinCondition,
                MovePieceAfterNMoves = config.MovePieceAfterNMoves
                
            };
        
        // Convert to Domain.Configuration
        public Configuration ToDbConfig() =>
            new Configuration
            {
                Id = this.Id, 
                Name = this.Name,
                BoardWidth = this.BoardWidth,
                BoardHeight = this.BoardHeight,
                GridSize = this.GridSize,
                WinCondition = this.WinCondition,
                MovePieceAfterNMoves = this.MovePieceAfterNMoves
            };
        
        
        
    }
    
}
