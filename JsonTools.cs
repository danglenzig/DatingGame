using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using DatingGame.Models;
using System.IO;


namespace DatingGame.JsonTools
{
    public static class CharacterDataReader
    {
        public static Characters? ReadCharactersFromFile()
        {
            try
            {
                // Resolve the path to data.json in the same folder as the running assembly
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.json");
                
                // read the file contents into a string
                string jsonContent = File.ReadAllText(filePath);

                // opt for case-insensitive
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                // return the constructed Characters object.
                return JsonSerializer.Deserialize<Characters>(jsonContent, options);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"### JsonTools Error: {e.Message}");
                return null;
            }



            

            // opt for case-insensitive
            

            
        }
    }
}
