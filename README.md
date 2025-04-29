# Tik-Tak-Dos  
TalTech c# Project Course 2024  

**Author:**  
Torm Järvelill  

## Introduction  
Tik-Tak-Dos is an implementation of a customized **Tic-Tac-Two** game, inspired by the board game reviewed [here](https://www.geekyhobbies.com/tic-tac-two-board-game-review/) and [described here](https://gamescrafters.berkeley.edu/games.php?game=tictactwo).  
This project was developed using **C#** and **.NET 8**, supporting both a **console-based** and a **web-based** user interface.

Key features include:
- Configurable board, grid, and win conditions.
- Save/load game states across different platforms (console ↔ web).
- Playable modes: two-player, player vs AI, AI vs AI.
- Interface-driven persistence layer supporting both file system (JSON) and database (SQLite) storage.


## Getting Started

### Requirements
- **.NET SDK 8.0** or later (latest stable version recommended)  
  Download from [https://dotnet.microsoft.com/en-us/download](https://dotnet.microsoft.com/en-us/download).

- **IDE of choice** (examples):
  - [JetBrains Rider](https://www.jetbrains.com/rider/) (preferred for full C# support)
  - [Visual Studio Code](https://code.visualstudio.com/) (requires C# extensions)

### Running the Console Version

1. Open the solution `Tik-Tak-Dos.sln` in your IDE.
2. Set the `ConsoleApp` as the **Startup Project**.
3. Build and run the project.
4. Play the game through the terminal:
   - Create custom game configurations.
   - Play using console visualization.
   - Save game state to file system (JSON) or database (SQLite).

### Running the Web Version

1. Open the same `Tik-Tak-Dos.sln`.
2. Set the `WebApp` as the **Startup Project**.
3. Build and run the project.
4. Open a browser and navigate to the hosted URL (e.g., `https://localhost:5001`).
5. Start new games or continue saved games.

The console and web applications **share** the same core business logic and data access layers.

## Project Structure

- `/GameBrain/` – Core game logic (moves, winning conditions, AI players).
- `/MenuSystem/` – Console menu system for configuration and game start.
- `/ConsoleUi/` – Console visualization layer.
- `/ConsoleApp/` – Main console application (controllers and entry point).
- `/WebApp/` – ASP.NET Core Razor Pages web application.
- `Tik-Tak-Dos.sln` –  Solution file linking all projects.

## Technologies Used

- **C#** – Main programming language
- **.NET 8 SDK** – Framework for building cross-platform applications
- **ASP.NET Core** – For web application (WebApp)
- **Entity Framework Core** – For SQLite database access
- **SQLite** – Lightweight embedded database
- **JSON file system storage** – Alternative game state storage
- **JetBrains Rider** / **VS Code** / **Visual Studio** – IDEs for development
- **Git** – Version control

## Architecture Overview

- **Separation of Concerns**:  
  Business logic (GameBrain) is completely separated from UI layers (Console and Web).
- **Persistence Layer**:  
  Uses interfaces to abstract between:
  - File system storage (JSON format)
  - Database storage (SQLite via Entity Framework Core)
- **Switchable Repositories**:  
  Changing between JSON and database saving requires only minimal code changes (based on injected repository implementation).

## Screenshots


Example:

### Console Gameplay
![image](https://github.com/user-attachments/assets/b202e65d-1822-4a83-b554-a79c73e82bd1)


### Web Gameplay
Starting a new game and choosing between configuration and gamemode
<img width="1415" alt="image" src="https://github.com/user-attachments/assets/e384fa58-b010-4735-bc36-67e14f11b9e6" />


Gameplay with option to move grid or place button
<img width="1389" alt="image" src="https://github.com/user-attachments/assets/09d75c32-70e4-442b-82d4-9c62562d8cc6" />



## Notes

- Game configurations are validated for reasonable board/grid sizes and piece counts.
- Game saving is flexible across console and web interfaces.
- Supports different gameplay modes including AI vs AI matches.

---
