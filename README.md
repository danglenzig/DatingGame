# Marvel Dating Game

A console-based compatibility quiz game featuring Marvel Cinematic Universe characters. Players answer personality questions to determine their dating compatibility score with their chosen character.

## Features

- **Interactive Quiz System**: Answer 10 compatibility questions per character (Yes/No/Maybe)
- **Scoreboard Integration**: Posts and retrieves scores via HTTP (Zapier/Google Sheets integration)
- **Character Profiles**: 8 Marvel characters with unique headlines and personality-based questions
- **Compatibility Scoring**: Calculates percentage compatibility based on answers (0-100%)
- **Score History**: View historical compatibility scores for any character

## Technical Highlights

**Network Programming Focus:**
- HTTP POST requests to post scores to external scoreboard
- HTTP GET requests to retrieve score history from scoreboard
- Async/await patterns for non-blocking network calls
- JSON serialization/deserialization for API communication

## How to Run

### Build
```bash
dotnet build
```

### Run
```bash
dotnet run
```

## Project Structure

- `Program.cs` - Application entry point
- `Game.cs` - Main game logic and flow control
- `DataModels.cs` - Data classes (DatingProfile, ProfileQuestion, ScoreEntry)
- `JsonTools.cs` - JSON file reading utility for character data
- `data.json` - Character profiles and questions database

## How to Play

1. Enter your player name
2. Select a character (1-8) or use options 9-10 for score lookup/quit
3. Answer 10 personality-based questions with Yes/No/Maybe responses
4. Your compatibility score is calculated and posted to the scoreboard
5. View past scores by selecting option 9

## Technologies Used

- **Language**: C#
- **Framework**: .NET 10.0
- **Networking**: HttpClient for REST API calls
- **Data Format**: JSON
- **Async Pattern**: Task-based asynchronous programming
