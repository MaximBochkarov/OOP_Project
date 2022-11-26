using System;
using Project_OOP.GameItems;

namespace Project_OOP.DataBase
{
    public class DBMethods
    {
        public static void GetAccInfo()
        {
            int accNumber = 1;
            foreach (var player in Program.DbContext.UsersList)
            {
                Console.WriteLine($"{accNumber}. Name: {player.UserName},\t acc-type: {player.GetType().Name},\trating: {player.CurrentRating}");
                accNumber++;
            }
        }
        public static void GetStats()
        {
            int gameNumber = 1;
            Console.WriteLine($"Total games: {Program.DbContext.GameHistory.Count}");
            foreach(var game in Program.DbContext.GameHistory)
            {
                Console.Write($"{gameNumber++}-({game.GameName})\t : index: {game.GameIndex} -->\t");
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
    }
}