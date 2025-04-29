using DAL;
using GameBrain;
using MenuSystem;

namespace ConsoleApp
{
    public static class GameController
    {
        
        private static IConfigRepository _configRepository;
        private static IGameRepository _gameRepository;
        private static TicTacTwoBrain _gameInstance;
        private static readonly IConfigRepository ConfigRepository = new ConfigRepositoryJson();
        public static EGamePiece StartingPlayer = EGamePiece.X;
        
        
        public static void Initialize(IConfigRepository configRepo, IGameRepository gameRepo)
        {
            _configRepository = configRepo;
            _gameRepository = gameRepo;
            Console.WriteLine("GameController initialized with repositories.");
        }

        public static string MainLoop()
        {
            var configName = ChooseConfiguration();

            var chosenConfig = _configRepository.GetConfigurationByName(configName);
            if (chosenConfig == null)
            {
                Console.WriteLine("Configuration not found.");
                return "Return";
            }
            StartNewGame(chosenConfig.Value);
            _gameInstance.GameMode = ChooseGameMode();
            RunGameLoop(); 
            return "Return";
        }
        
        public static void StartNewGame(GameConfiguration config)
        {
            Console.WriteLine($"Starting new game with BoardSizeWidth: {config.BoardWidth}, BoardSizeHeight: {config.BoardHeight}");
            _gameInstance = new TicTacTwoBrain(config);
            _gameInstance.SetStartingPlayer(StartingPlayer);
            
        }
        
        public static void SaveGame()
        {
            
            var gameState = _gameInstance.GetGameState();
            gameState.TotalMovesMade = _gameInstance.TotalMovesMade; 
            _gameRepository.SaveGame(gameState, 0);

        }
        
        public static void LoadGame(int saveGameId)
        {
            var gameState = _gameRepository.LoadGame(saveGameId);
            if (gameState != null)
            {
                _gameInstance = new TicTacTwoBrain(gameState.GameConfiguration);
                _gameInstance.LoadGameState(gameState);
                Console.WriteLine("Game loaded successfully.");
            }
            else
            {
                Console.WriteLine("Failed to load game.");
            }
        }
        
        private static void RunGameLoop()
{
    while (true)
    {
        ConsoleUI.Visualizer.DrawBoard(_gameInstance);
        
        if (_gameInstance.GameMode == "PlayerVsAI" && _gameInstance.GetCurrentPlayer() == EGamePiece.O)
        {
            // AI's Turn
            Console.WriteLine("AI's Turn");
            _gameInstance.MakeAiMove(EGamePiece.O); // AI makes its move
            ConsoleUI.Visualizer.DrawBoard(_gameInstance); // Redraw the board after AI's move

            // After AI's move, check for a winner and break if there's one
            if (_gameInstance.CheckWinCondition() != null)
            {
                var winner = _gameInstance.CheckWinCondition();
                Console.WriteLine($"Player {winner} wins!");
                break;
            }
            _gameInstance.SwitchPlayer();
            ;
        }
        else if (_gameInstance.GameMode == "AiVsAi")
        {
            Console.WriteLine($"AI ({_gameInstance.GetCurrentPlayer()})'s Turn");
            _gameInstance.MakeAiMove(_gameInstance.GetCurrentPlayer());
            ConsoleUI.Visualizer.DrawBoard(_gameInstance);

            // Check for win or draw after each move
            var winner = _gameInstance.CheckWinCondition();
            if (winner != null)
            {
                Console.WriteLine($"Player {winner} wins!");
                break;
            }
            
            _gameInstance.SwitchPlayer(); // Switch turn to the other AI
        }

        
        
        DisplayAvailableCommands();
        
        var command = Console.ReadLine()?.ToLower();
        
        if (command == "save")
        {
            SaveGame();
        }
        else if (command == "load")
        {
            Console.Write("Enter save game ID to load the game: ");
            if (int.TryParse(Console.ReadLine(), out var saveGameId))
            {
                LoadGame(saveGameId);
            }
            else
            {
                Console.WriteLine("Invalid save game ID. Please enter a valid number.");
            }
        }
        else if (command == "exit")
        {
            break;
        }
        else if (!ProcessCommand(command))
        {
            Console.WriteLine("Invalid command. Try again.");
        }
        

        // After player's move, check for a winner
        if (_gameInstance.CheckWinCondition() != null)
        {
            var winner = _gameInstance.CheckWinCondition();
            Console.WriteLine($"Player {winner} wins!");
            break;
        }

        // Check for draw (if the board is full and no winner)
        if (_gameInstance.TotalMovesMade == _gameInstance.DimX * _gameInstance.DimY)
        {
            Console.WriteLine("It's a draw!");
            break;
        }
    }
}
        private static bool ProcessCommand(string command)
        {
            switch (command)
            {
                case "place":
                    return PlacePiece();
                case "move piece":
                    return MovePiece();
                case "move grid":
                    return MoveGrid();
                default:
                    return false;
            }
        }

        private static bool PlacePiece()
        {
            Console.Write("Enter coordinates to place your piece (x,y): ");
            var input = Console.ReadLine()?.Split(",");
            if (input == null || input.Length != 2 || 
                !int.TryParse(input[0], out var x) || 
                !int.TryParse(input[1], out var y))
            {
                Console.WriteLine("Invalid coordinates. Try again.");
                return false;
            }
    
            bool success = _gameInstance.MakeAMove(x, y); 

            if (success)
            {
                ConsoleUI.Visualizer.DrawBoard(_gameInstance); // Redraw the board after placing the piece
            }

            return success;
        }

        private static bool MovePiece()
        {
            Console.Write("Enter piece coordinates to move (x,y): ");
            var from = Console.ReadLine()?.Split(",");
            if (from == null || from.Length != 2 ||
                !int.TryParse(from[0], out var x) ||
                !int.TryParse(from[1], out var y))
            {
                Console.WriteLine("Invalid coordinates. Try again.");
                return false;
            }
            Console.Write("Enter new coordinates (x,y): ");
            var to = Console.ReadLine()?.Split(",");
            return to != null && to.Length == 2 &&
                   int.TryParse(to[0], out var newX) &&
                   int.TryParse(to[1], out var newY) &&
                   _gameInstance.MovePiece(x, y, newX, newY);
                  return true;
        }

        private static bool MoveGrid()
        {
            Console.Write("Enter grid movement (dx,dy): ");
            var movement = Console.ReadLine()?.Split(",");
            return movement != null && movement.Length == 2 &&
                   int.TryParse(movement[0], out var dx) &&
                   int.TryParse(movement[1], out var dy) &&
                   _gameInstance.MoveGrid(dx, dy);
        }

        private static string ChooseConfiguration()
        {
            if (_configRepository == null)
            {
                throw new InvalidOperationException("ConfigRepository is not initialized.");
            }

            var configNames = _configRepository.GetConfigurationNames();
            if (!configNames.Any())
            {
                Console.WriteLine("No configurations available.");
                return "";
            }

            var configMenuItems = new List<MenuItem>();
            for (var i = 0; i < configNames.Count; i++)
            {
                var returnValue = configNames[i];
                configMenuItems.Add(new MenuItem()
                {
                    Title = configNames[i],
                    Shortcut = (i + 1).ToString(),
                    MenuItemAction = () => returnValue
                });
            }

            var configMenu = new Menu(EMenuLevel.Deep, "TIC-TAC-TWO - Choose Game Configuration", configMenuItems);
            var selectedConfig = configMenu.Run();

            // Debug: print selected configuration name
            Console.WriteLine($"Selected configuration name: {selectedConfig}");

            return selectedConfig;
        }

        public static string CreateNewConfiguration()
        {
            Console.WriteLine("Enter a name for your game configuration:");
            var name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Configuration name cannot be empty.");
                return "Return";
            }

            int boardWidth, boardHeight, gridSize, winCondition;
            
            // Input for board width and height
            do
            {
                Console.WriteLine("Enter board width (3-20):");
                boardWidth = int.TryParse(Console.ReadLine(), out var width) ? width : 0;
                Console.WriteLine("Enter board height (3-20):");
                boardHeight = int.TryParse(Console.ReadLine(), out var height) ? height : 0;

                if (boardWidth < 3 || boardHeight < 3 || boardWidth > 20 || boardHeight > 20)
                {
                    Console.WriteLine("Please enter valid dimensions between 3 and 20.");
                }
            } while (boardWidth < 3 || boardHeight < 3 || boardWidth > 20 || boardHeight > 20);

            // Input for grid size
            do
            {
                Console.WriteLine("Enter grid size (<= board size):");
                gridSize = int.TryParse(Console.ReadLine(), out var grid) ? grid : 0;

                if (gridSize > boardWidth || gridSize > boardHeight)
                {
                    Console.WriteLine("Grid size must be less than or equal to board dimensions.");
                }
            } while (gridSize > boardWidth || gridSize > boardHeight || gridSize <= 0);

            // Input for win condition
            do
            {
                Console.WriteLine($"Enter the number of pieces required to win (<= grid size, max {gridSize}):");
                winCondition = int.TryParse(Console.ReadLine(), out var win) ? win : 0;

                if (winCondition > gridSize || winCondition < 3)
                {
                    Console.WriteLine("Win condition must be within grid size and at least 3.");
                }
            } while (winCondition > gridSize || winCondition < 3);

            // Create and save the configuration
            var newConfig = new GameConfiguration
            {
                Name = name,
                BoardWidth = boardWidth,
                BoardHeight = boardHeight,
                GridSize = gridSize,
                WinCondition = winCondition
            };

            _configRepository.SaveConfiguration(newConfig);
            Console.WriteLine("New configuration created and saved successfully!");

            return "Return";
        }
        
        private static string ChooseGameMode()
        {
            Console.WriteLine("Choose Game Mode: ");
            Console.WriteLine("1. Two Player");
            Console.WriteLine("2. Player vs AI");
            Console.WriteLine("3. AI vs AI");
            var mode = Console.ReadLine()?.Trim();

            if (mode == "2")
            {
                return "PlayerVsAI";
            }
            else if (mode == "3")
            {
                return "AIvsAI";
            }
            else
            {
                return "TwoPlayer"; 
            }
        }
        
        private static void DisplayAvailableCommands()
        {
            Console.WriteLine("Enter command ('place', 'save', 'load', 'exit'): ");
            
            if (_gameInstance.TotalMovesMade >= 4)
            {
                Console.WriteLine("Enter command ('place', 'move grid', 'save', 'load', 'exit): ");
            }
            
        }
    }
}
