using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace DatingGame.Models
{

    public class Characters
    {
        [JsonPropertyName("char_list")]
        public List<DatingProfile> CharList { get; set; } = new();
    }

    public class DatingProfile
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("headline")]
        public string Headline { get; set; } = string.Empty;

        [JsonPropertyName("questions")]
        public List<ProfileQuestion> Questions { get; set; } = new();
    }

    public class ProfileQuestion
    {
        [JsonPropertyName("question_text")]
        public string QuestionText { get; set; } = string.Empty;

        [JsonPropertyName("correct_answer")]
        public string CorrectAnswer { get; set; } = "";
    }

    public class YesNoMaybe
    {
        public static string YES = "yes";
        public static string NO = "no";
        public static string MAYBE = "maybe";
    }

    public static class CharacterListDisplay
    {
        public static string DisplayText = "1.  Iron Man\n2.  The Black Widow\n3.  Thor\n4.  The Scarlet Witch\n5.  The Hulk\n6.  Gamora\n7.  Captain America\n8.  Captain Marvel\n9.  (get score)\n10. (quit)\n\n";
    }

    public static class GameTitleDisplay
    {
        public static string TitleStr = "  __  __  ____ _   _    ____    _  _____ ___ _   _  ____ \n" +
" |  \\/  |/ ___| | | |  |  _ \\  / \\|_   _|_ _| \\ | |/ ___|\n" +
" | |\\/| | |   | | | |  | | |  / _ \\ | |  | ||  \\| | |  _ \n" +
" | |  | | |___| |_| |  | |_| / ___ \\| |  | || |\\  | |_| |\n" +
" |_|  |_|\\____|\\___/   |____/_/   \\_\\_| |___|_| \\_|\\____|\n\n " +
"     ~~*~~  G  A  M  E     O  F     L  O  V  E  ~~*~~\n\n";
    }
    public class ScoreEntry
    {
        public string Name { get; set; } = "NONE";
        public int Score { get; set; } = -1;
    }

}
