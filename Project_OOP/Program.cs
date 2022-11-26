using System;
using Project_OOP.DataBase;
using Project_OOP.GameItems;

namespace Project_OOP
{
    internal class Program
    {
        public static readonly DBContext DbContext = new DBContext();
        
        public static void Main(string[] args)
        {
            var player1 = new ThriftyGameAccount("Solify");
            var player3 = new ExtraSeriesPointsGameAccount("Chokopie");

            GameVariety.GetStandardGame(player1, player3, 10);
            GameVariety.GetStandardGame(player1, player3, 20);
            GameVariety.GetStandardGame(player1, player3, 20);
            GameVariety.GetSoloRankedGame(player3, 40);
            
            
            DataHandling.GetStats();
            Console.WriteLine("------------------------------------------------");
            DataHandling.GetAccInfo();
        }
    }
}