namespace DAL;

public class FileHelper
{
    public static readonly string BasePath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
        "tic-tac-toe"
    );

    public static readonly string ConfigExtension = ".config.json";
    public static readonly string GameExtension = ".game.json";
}
