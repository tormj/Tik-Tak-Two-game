using MenuSystem;
using DAL;
using GameBrain;

namespace ConsoleApp
{
    public static class Menus
    {
        // Define the Options Menu
        public static readonly Menu OptionsMenu = new Menu(
            EMenuLevel.Secondary,
            "TIC-TAC-TWO Options",
            new List<MenuItem>
            {
                new MenuItem
                {
                    Shortcut = "C",
                    Title = "Create New Configuration",
                    MenuItemAction = GameController.CreateNewConfiguration
                },
                 new MenuItem
                {
                    Shortcut = "X",
                    Title = "X Starts",
                    MenuItemAction = () =>
                    {
                        GameController.StartingPlayer = EGamePiece.X;
                        Console.WriteLine("Player X will start the game.");
                        return "Return";
                    }
                },
                new MenuItem
                {
                    Shortcut = "O",
                    Title = "O Starts",
                    MenuItemAction = () =>
                    {
                        GameController.StartingPlayer = EGamePiece.O;
                        Console.WriteLine("Player O will start the game.");
                        return "Return";
                    }
                },
            }
        );

        // Define the Main Menu
        public static Menu MainMenu = new Menu(
            EMenuLevel.Main,
            "TIC-TAC-DOS",
            new List<MenuItem>
            {
                new MenuItem
                {
                    Shortcut = "N",
                    Title = "New Game",
                    MenuItemAction = GameController.MainLoop
                },
                new MenuItem
                {
                    Shortcut = "O",
                    Title = "Options",
                    MenuItemAction = OptionsMenu.Run
                },
            }
        );

        private static string DummyMethod()
        {
            Console.Write("Just press any key to get out from here!");
            Console.ReadKey();
            return "foobar";
        }
    }
}
