using DatingGame.JsonTools;
using DatingGame.Models;
using System;
using System.Net.Http.Json;

namespace DatingGame
{
    public class Game
    {

        private const string SCOREBOARD_POST_ENDPOINT = "https://hooks.zapier.com/hooks/catch/8338993/ujs9jj9/";
        private const string SCOREBOARD_GET_ENDPOINT = "https://script.google.com/macros/s/AKfycbys5aEPMvNCutyhNYYCcQcCjzsi2UtqNspmKyCH-AicJxJbCJMrAoT0LUaYaXhTWA8n/exec";

        private HttpClient CLIENT = new HttpClient();

        Characters? CHARACTERS = CharacterDataReader.ReadCharactersFromFile();

        private Dictionary<int, string> CHAR_DICT = new Dictionary<int, string>
        {
            {1, "Tony Stark" },
            {2, "Natasha Romanoff" },
            {3, "Thor Odinson" },
            {4, "Wanda Maximoff" },
            {5, "Bruce Banner / The Hulk" },
            {6, "Gamora" },
            {7, "Steve Rogers"},
            {8, "Carol Danvers" },
        };

        private bool TryPostScoreData(ScoreEntry data)
        {
            try
            {
                HttpResponseMessage? postResponse = CLIENT.PostAsJsonAsync(SCOREBOARD_POST_ENDPOINT, data).Result;
                Console.WriteLine(postResponse?.ToString());
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        private void WriteToScoreboard(string player_name, int score)
        {
            ScoreEntry entry = new ScoreEntry
            {
                Name = player_name,
                Score = score,
            };
            bool isSuccess = TryPostScoreData(entry);

            if (isSuccess)
            {
                Console.WriteLine("\nYour score was posted");
                return;
            }
            else
            {
                Console.WriteLine("\nThere was a problem posting your score.");
                return;
            }
        }

        async Task<List<ScoreEntry>?> TryGetScoresList()
        {
            List<ScoreEntry>? scoresList = new List<ScoreEntry>();

            try
            {
                scoresList = await CLIENT.GetFromJsonAsync<List<ScoreEntry>>(SCOREBOARD_GET_ENDPOINT);
                return scoresList;
            }
            catch
            {
                return scoresList;
            }
        }

        async private Task<string> TryGetResult(string player_name)
        {
            string result = "FOO";
            List<ScoreEntry>? scores_list = new List<ScoreEntry>();

            if (CHARACTERS == null)
            {
                Console.WriteLine($"### Game: the character catalogue is empty or null");
                return result;
            }

            try
            {
                scores_list = await TryGetScoresList();
            }
            catch (Exception e)
            {
                Console.WriteLine($"### Game: there was a roblem getting the scores list: {e}");
                return result;
            }

            if ((scores_list != null && scores_list.Count <= 0) || scores_list == null)
            {
                Console.WriteLine($"### Game: the scores list is empty or null");
                return result;
            }

            int input_number = -1;
            while (input_number < 1 || input_number > CHARACTERS.CharList.Count)
            {
                Console.WriteLine($"\nFor which character?\n");
                Console.WriteLine(CharacterListDisplay.DisplayText);
                Console.Write("Enter a number --> ");

                string input_number_str = Console.ReadLine()!;
                if (input_number_str == "quit") Environment.Exit(0);

                input_number = Convert.ToInt32(input_number_str);

                if (input_number >= 1 && input_number <= CHARACTERS.CharList.Count)
                {
                    string choice_name = CHAR_DICT[input_number];
                    
                    DatingProfile? profile = CHARACTERS.CharList.Find(p => p.Name == choice_name);
                    if (profile == null)
                    {
                        Console.WriteLine($"Game: there was a problem getting the profile for {choice_name}");
                        return result;
                    }

                    List<ScoreEntry> candidate_scores = new List<ScoreEntry>();
                    foreach (ScoreEntry entry in scores_list)
                    {
                        string name_str = $"{player_name}'s % dating compatability with {choice_name}";
                        if (entry.Name == name_str)
                        {
                            candidate_scores.Add(entry);
                        }
                    }

                    if (candidate_scores.Count <= 0)
                    {
                        Console.WriteLine($"### Game: {player_name} has no score record for {choice_name} yet.");
                        return result;
                    }

                    int highest_score = 0;
                    ScoreEntry best_score_entry = new ScoreEntry();

                    foreach (ScoreEntry entry in candidate_scores)
                    {
                        if (entry.Score > highest_score)
                        {
                            best_score_entry = entry;
                        }
                    }

                    return $"*** {best_score_entry.Name}: {best_score_entry.Score} ***";
                }
                else break;
            }
            return result;
        }

        private float AskQuestions(string player_name, DatingProfile profile)
        {
            string char_name = profile.Name;
            string headline = profile.Headline;
            List<ProfileQuestion> questions = profile.Questions;
            int number_of_questions = questions.Count;
            int max_correct = number_of_questions * 2;

            Console.WriteLine($"\nHi {player_name}! I'm {char_name}.\n");
            Console.WriteLine($"{headline}\nHere are my questions for you...\n");

            float player_score = 0.0f;

            for (int i = 0; i < number_of_questions; i++)
            {
                ProfileQuestion question = questions[i];
                string question_text = question.QuestionText;
                string answer_text = question.CorrectAnswer;

                int players_answer = -1;

                while (players_answer < 1 || players_answer > 3)
                {
                    Console.WriteLine($"\n{i + 1}. {question_text}\n");
                    Console.WriteLine("\n1. Yes\n2. No\n3. Maybe\n\n");
                    Console.Write("Enter a number --> ");
                    string input_number_str = Console.ReadLine()!;
                    if (input_number_str == "quit") Environment.Exit(0);
                    players_answer = Convert.ToInt32(input_number_str);
                }
                float question_score = -1.0f;
                switch (players_answer)
                {
                    case 1:
                        // "yes";
                        question_score = answer_text == "yes" ? 2.0f : 0.0f;
                        break;
                    case 2:
                        // "no";
                        question_score = answer_text == "yes" ? 0.0f : 2.0f;
                        break;
                    case 3:
                        // "maybe";
                        question_score = 1.0f;
                        break;
                    default:
                        break;
                }
                player_score += question_score;
            }
            float percent_score = player_score / max_correct;
            return MathF.Round(percent_score, 2);
        }

        public async Task RunGame()
        {
            if (CHARACTERS == null)
            {
                Console.WriteLine("### Program Error: Failed read from file");
                return;
            }

            Console.WriteLine(GameTitleDisplay.TitleStr);

            Console.Write("Enter your name --> ");
            string player_name = Console.ReadLine()!;
            if (player_name == "quit") Environment.Exit(0);

            int input_number = -1;

            while (input_number < 1 || input_number > CHARACTERS.CharList.Count)
            {
                Console.WriteLine($"\nWhich character from the Marvel Cinematic Universe are you tryna' date, {player_name}?\n");
                Console.WriteLine(CharacterListDisplay.DisplayText);
                Console.Write("Enter a number --> ");

                string input_number_str = Console.ReadLine()!;                
                input_number = Convert.ToInt32(input_number_str);

                
                if (input_number == CHARACTERS.CharList.Count + 1)
                {
                    string score = await TryGetResult(player_name);
                    Console.WriteLine(score);
                    Environment.Exit(0);
                }

                if (input_number == CHARACTERS.CharList.Count + 2)
                {
                    Console.WriteLine("Bye!");
                    Environment.Exit(0);
                }


                if (input_number >= 1 && input_number <= CHARACTERS.CharList.Count)
                {
                    string choice_name = CHAR_DICT[input_number];
                    DatingProfile? profile = CHARACTERS.CharList.Find(p => p.Name == choice_name);

                    if (profile == null) break;
                    float score = AskQuestions(player_name, profile);

                    string scoreboard_string = $"{player_name}'s % dating compatability with {choice_name}";

                    Console.WriteLine($"\n*** {player_name}'s dating compatability with {choice_name} is {score * 100}% ***\n\n");

                    int int_score = (int)Math.Round(score * 100, 0);


                    WriteToScoreboard(scoreboard_string, int_score);

                    break;
                }
                else break;
            }
        }
    }
}
