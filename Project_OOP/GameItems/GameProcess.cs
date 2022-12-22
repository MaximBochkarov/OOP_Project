using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Project_OOP.DataBase;

namespace Project_OOP.GameItems
{
    public class GameProcess
    {
        public GameProcess()
        {
            
        }
        public void Start()
        {
            DbContext database;
            while (true)
            {
                database = DataHandling.GetData();
                Console.Write("<------------------------------>" +
                              "\n 1) Create a new account. " +
                              "\n 2) Delete existing account " +
                              "\n 3) Play the game. " +
                              "\n 4) Show account stats. " +
                              "\n 5) Show game history. " +
                              "\n 6) Close the program. " +
                              "\n<------------------------------>" +
                              "\n --> ");
                InitialChoice(NumberChoice(), database);
                DataHandling.SaveData(database);
            }
            
        }
        private void InitialChoice(int decide, DbContext database)
        {
            switch (decide)
            {
                case 1:
                    AccountCreation(database);
                    Thread.Sleep(3000);
                    Console.Clear();
                    break;
                case 2:
                    if (!AmountOfPlayers(1, database))
                    {
                        Console.WriteLine("No account created!");
                        return;
                    }
                    DeleteUser(database);
                    Thread.Sleep(3000);
                    Console.Clear();
                    break;
                case 3:
                    GenerateGame(database);
                    break;
                case 4:
                    DataHandling.GetAccInfo(database);
                    break;
                case 5:
                    DataHandling.GetStats(database);
                    break;
                case 6:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Options (1 - 6) \n");
                    break;
            }
        }

        private void GenerateGame(DbContext database)
        {
            Console.WriteLine("Types: 1. Standard; 2. Practice; 3. SoloRanked; (4 - return to menu)");
            Console.Write("--> ");
            int decide = NumberChoice();
            switch (decide)
            {
                case -1:
                case 4:
                    return;
            }

            int rating;
            GameAccount acc1, acc2;

            switch (decide)
            {
                case 1:
                    if (!AmountOfPlayers(2, database))
                    {
                        Console.WriteLine("At least two players required");
                        return;
                    }
                    Console.Write("Enter game rating: ");
                    rating = NumberChoice();
                    if (rating == -1) return;
                    acc1 = FindAccount(SetUsername(database), database);
                    acc2 = FindAccount(SetUsername(database), database);
                    if (AccountsIdentical(acc1, acc2))
                    {
                        Console.WriteLine("Cannot play with yourself!");
                        return;
                    }
                    SaveGame(GameVariety.GetStandardGame(acc1, acc2, rating), database);
                    break;
                case 2:
                    if (!AmountOfPlayers(2, database))
                    {
                        Console.WriteLine("At least two players required");
                        return;
                    }
                    acc1 = FindAccount(SetUsername(database), database);
                    acc2 = FindAccount(SetUsername(database), database);
                    if (AccountsIdentical(acc1, acc2))
                    {
                        Console.WriteLine("Cannot play with yourself!");
                        return;
                    }
                    SaveGame(GameVariety.GetPracticeGame(acc1, acc2), database);
                    break;
                case 3:
                    if (!AmountOfPlayers(1, database))
                    {
                        Console.WriteLine("At least one player required");
                        return;
                    }
                    Console.Write("Enter game rating: ");
                    rating = NumberChoice();
                    if (rating == -1) return;
                    SaveGame(GameVariety.GetSoloRankedGame(FindAccount(SetUsername(database), database), rating), database);
                    break;
                default:
                    Console.WriteLine("Options (1 - 4) \n");
                    break;
            }
        }

        private static GameAccount FindAccount(string username, DbContext database)
        {
            return database.UsersList.FirstOrDefault(user => user.UserName.Equals(username));
        }
        private void AccountCreation(DbContext database)
        {
            Console.WriteLine("Types: 1. Default; 2. Thrifty; 3. Premium; 4. ExtraSeriesPoints (5 - return to menu)");
            Console.Write("--> ");
            int decide = NumberChoice();
            switch (decide)
            {
                case -1:
                case 5:
                    return;
            }

            string username;

            switch (decide)
            {
                case 1:
                    username = GetUsername(database);
                    SaveAcc(new GameAccount(username), database);
                    Console.WriteLine($"Congrats {username}, the default account was created!");
                    break;
                case 2:
                    username = GetUsername(database);
                    SaveAcc(new ThriftyGameAccount(username), database);
                    Console.WriteLine($"Congrats {username}, the thrifty account was created!");
                    break;
                case 3:
                    username = GetUsername(database);
                    SaveAcc(new PremiumGameAccount(username), database);
                    Console.WriteLine($"Congrats {username}, the Premium account was created!");
                    break;
                case 4:
                    username = GetUsername(database);
                    SaveAcc(new ExtraSeriesPointsGameAccount(username), database);
                    Console.WriteLine($"Congrats {username}, the ExtraSeriesPointsGameAccount account was created!");
                    break;
                default:
                    Console.WriteLine("Options (1 - 5) \n");
                    break;
            }
        }

        private static string SetUsername(DbContext database)
        {
            string username;
            while (true)
            {
                Console.Write("Enter account username: ");
                username = Console.ReadLine();
                if (UserExists(username, database))
                {
                    break;
                }

                Console.WriteLine($"Unable to find user with name: '{username}'.");
            }
            return username;
        }

        private static string GetUsername(DbContext database)
        {
            string username;
            while (true)
            {
                Console.Write("Create a username: ");
                username = Console.ReadLine()?.Trim();
                if (UserExists(username, database))
                {
                    Console.WriteLine("Username is already engaged!");
                }
                else
                {
                    break;
                }
            }
            return username;
        }
        private static int NumberChoice()
        {
            int number = -1;
            try
            {
                number = Convert.ToInt32(Console.ReadLine());
            }
            catch (FormatException e)
            {
                Console.WriteLine("Only numbers");
                Console.WriteLine(e.ToString());
            }
            return number;
        }
        private static void SaveAcc(GameAccount acc, DbContext database)
        {
            database.UsersList.Add(acc);
        }
        private static void SaveGame(Game game, DbContext database)
        {
            database.GameHistory.Add(game.HistoryGame);
        }

        private static bool UserExists(string username, DbContext database)
        {
            return database.UsersList.Any(user => user.UserName.Equals(username));
        }

        private static void DeleteUser(DbContext database)
        {
            database.UsersList.Remove(FindAccount(SetUsername(database), database));
            Console.WriteLine("Account was successfully removed!");
        }

        private static bool AmountOfPlayers(int amount, DbContext database)
        {
            return database.UsersList.Count >= amount;
        }

        private static bool AccountsIdentical(GameAccount acc1, GameAccount acc2)
        {
            return acc1 == acc2;
        }
    }
}