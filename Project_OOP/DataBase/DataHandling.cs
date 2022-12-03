using System;
using Project_OOP.GameItems;
using System.IO;
using Newtonsoft.Json;

namespace Project_OOP.DataBase
{
    public static class DataHandling
    {
        public static void GetAccInfo(DbContext database)
        {
            int accNumber = 1;
            Console.WriteLine($"Total players: {database.UsersList.Count}");
            foreach (var player in database.UsersList)
            {
                Console.WriteLine($"{accNumber}. Name: {player.UserName}, \trating: {player.CurrentRating}, \tacc-type: {player.GetType().Name}");
                accNumber++;
            }
        }
        public static void GetStats(DbContext database)
        {
            int gameNumber = 1;
            Console.WriteLine($"Total games: {database.GameHistory.Count}");
            foreach(var game in database.GameHistory)
            {
                Console.Write($"{gameNumber++}-({game.GameName}): \tindex: {game.GameIndex} -->\t");
                switch (game.GameStatus)
                {
                    case GameStatus.Win:
                        Console.WriteLine($"Winner: '{game.CurrentAccount.UserName}', \tLooser: '{game.Opponent.UserName}', \tgame rating: {game.Rating}.");
                        break;
                    case GameStatus.Lose:
                        Console.WriteLine($"Winner: '{game.Opponent.UserName}', \tLooser: '{game.CurrentAccount.UserName}', \tgame rating: {game.Rating}.");
                        break;
                    case GameStatus.Draw:
                        Console.WriteLine($"Draw:   '{game.CurrentAccount.UserName}' and '{game.Opponent.UserName}' tied.");
                        break;
                    default:
                        Console.WriteLine("Game status is unknown");
                        break;
                }
            }
            
        }
        public static void SaveData(DbContext dbContext)
        {
            var dbContextJson = JsonConvert.SerializeObject(dbContext, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            File.WriteAllText("DataBaseSerialization.json", dbContextJson);
        }
        public static DbContext GetData()
        {
            var dbContextJson = File.ReadAllText("DataBaseSerialization.json");
            var db = JsonConvert.DeserializeObject<DbContext>(dbContextJson, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            return db ?? new DbContext();
        }
    }
}