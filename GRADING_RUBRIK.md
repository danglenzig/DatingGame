# 05 — Assignment: Online Scoreboard Client

## Goal

Build a complete C# console client that can both:

- submit scores using `POST`
- retrieve scores using `GET`

This exercise is less guided than the previous ones.

---

# Ticket: Online Scoreboard Client

Build a console application that communicates with the provided scoreboard API.

The application should start with a menu similar to:

```text
=== ONLINE SCOREBOARD ===

1. Submit Score
2. View Scoreboard
3. Exit

Choose:
```

---

## Requirement 1 — Submit a score

When the user selects:

```text
1. Submit Score
```

ask for:

```text
Name:
Score:
```

Then submit the data to:

```text
POST
https://hooks.zapier.com/hooks/catch/8338993/ujs9jj9/
```

The JSON structure must be:

```json
{
  "name": "Player Name",
  "score": 500
}
```

Example output:

```text
Name: Ben
Score: 5600

Submitting...

Score submitted!
```

---

## Requirement 2 — View the scoreboard

When the user selects:

```text
2. View Scoreboard
```

retrieve data from:

```text
GET
https://script.google.com/macros/s/AKfycbys5aEPMvNCutyhNYYCcQcCjzsi2UtqNspmKyCH-AicJxJbCJMrAoT0LUaYaXhTWA8n/exec
```

Deserialize it into:

```csharp
List<ScoreEntry>
```

Then display the scores from highest to lowest.

Example:

```text
=== LEADERBOARD ===

1. Alice        8300
2. Dave         7200
3. Ben          5600
4. Charlie      4500
```

---

## Requirement 3 — Use a model

Create:

```csharp
public class ScoreEntry
{
    public string Name { get; set; } = "";
    public int Score { get; set; }
}
```

Use this type for both sending and receiving scoreboard data.

---

## Requirement 4 — Do not crash on network errors

A network request can fail.

Examples:

- no internet connection
- server unavailable
- incorrect URL
- timeout

Handle at least:

```csharp
HttpRequestException
```

Example user-friendly output:

```text
Could not connect to the server.
```

The application should continue running instead of crashing.

---

## Requirement 5 — Exit cleanly

The program should continue showing the menu until the user chooses:

```text
3. Exit
```

---

# Minimum requirements checklist
Complete ALL for G:
- [ ] Menu works
- [ ] User can enter a name
- [ ] User can enter a score
- [ ] Score is sent using `POST`
- [ ] Scoreboard is retrieved using `GET`
- [ ] JSON is deserialized into C# objects
- [ ] Scores are sorted highest to lowest
- [ ] Network errors do not immediately crash the program
- [ ] User can exit the application

---

# Bonus challenges

Complete all minimum requirements & at least 2 bonus challenges for VG:

### #1 Top 10

Display only the ten highest scores.

### #2 Personal best

Ask for a player name and display their highest submitted score.

### #3 Better validation

Reject:

- blank names
- invalid numeric input
- negative scores

### #4 Separate the networking code

Create a class such as:

```csharp
public class ScoreboardApi
{
}
```

Move the HTTP logic out of `Program.cs`.

The rest of the application should then be able to call methods such as:

```csharp
await api.SubmitScore(...);
await api.GetScoreboard();
```

### #5 Loading state

Display:

```text
Loading scoreboard...
```

while waiting for the request.

### #6 Firebase Realtime Database

Instead of the given API endpoints, create your own database.

```text
scores
 ├── Oabc123xyz
 │     ├── name: "Alice"
 │     └── score: 9000
 │
 └── 1abc123xyz
       ├── name: "Bob"
       └── score: 5000
```

### Hints:
Check endpoint format:
```csharp
string url = "https://YOUR_DATABASE_URL/scores.json"
```

Check Firebase Rules:

```json
{
  "rules": {
    ".read": true,
    ".write": true
  }
}
```
To be used ONLY while testing as it greatly compromises the security of your database.

---

# Think about it

Your application is now doing something a local-only game cannot do:

```text
Student A
    │
    │ POST score
    ▼
Remote storage
    │
    │ GET
    ▼
Student B
```

Data created on one computer becomes available to another computer over the network.