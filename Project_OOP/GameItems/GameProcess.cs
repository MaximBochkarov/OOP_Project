using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Project_OOP.DataBase;
using System.Security.Cryptography;

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

        private static void GenerateGame(DbContext database)
        {
            Console.WriteLine("Types: 1. Standard; 2. Practice; 3. SoloRanked; (4 - return to menu)");
            Console.Write(" --> ");
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
                    if (!SignIn(acc1.Password)) return;
                    acc2 = FindAccount(SetUsername(database), database);
                    if (AccountsIdentical(acc1, acc2))
                    {
                        Console.WriteLine("Cannot play with yourself!");
                        return;
                    }
                    if (!SignIn(acc2.Password)) return;
                    
                    SaveGame(GameVariety.GetStandardGame(acc1, acc2, rating), database);
                    break;
                case 2:
                    if (!AmountOfPlayers(2, database))
                    {
                        Console.WriteLine("At least two players required");
                        return;
                    }
                    acc1 = FindAccount(SetUsername(database), database);
                    if (!SignIn(acc1.Password)) return;
                    acc2 = FindAccount(SetUsername(database), database);
                    if (AccountsIdentical(acc1, acc2))
                    {
                        Console.WriteLine("Cannot play with yourself!");
                        return;
                    }
                    if (!SignIn(acc2.Password)) return;
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
                    acc1 = FindAccount(SetUsername(database), database);
                    if (!SignIn(acc1.Password)) return;
                    SaveGame(GameVariety.GetSoloRankedGame(acc1, rating), database);
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
        private static void AccountCreation(DbContext database)
        {
            Console.WriteLine("Types: 1. Default; 2. Thrifty; 3. Premium; 4. ExtraSeriesPoints (5 - return to menu)");
            Console.Write(" --> ");
            int decide = NumberChoice();
            switch (decide)
            {
                case -1:
                case 5:
                    return;
            }

            BasicAccData basicAccData;

            switch (decide)
            {
                case 1:
                    basicAccData = SignUp(database);
                    SaveAcc(new GameAccount(basicAccData.Name, basicAccData.Password), database);
                    Console.WriteLine($"Congrats {basicAccData.Name}, the default account was created!");
                    break;
                case 2:
                    basicAccData = SignUp(database);
                    SaveAcc(new ThriftyGameAccount(basicAccData.Name, basicAccData.Password), database);
                    Console.WriteLine($"Congrats {basicAccData.Name}, the thrifty account was created!");
                    break;
                case 3:
                    basicAccData = SignUp(database);
                    SaveAcc(new PremiumGameAccount(basicAccData.Name, basicAccData.Password), database);
                    Console.WriteLine($"Congrats {basicAccData.Name}, the Premium account was created!");
                    break;
                case 4:
                    basicAccData = SignUp(database);
                    SaveAcc(new ExtraSeriesPointsGameAccount(basicAccData.Name, basicAccData.Password), database);
                    Console.WriteLine($"Congrats {basicAccData.Name}, the ExtraSeriesPointsGameAccount account was created!");
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

        private static BasicAccData SignUp(DbContext database)
        {
            string username = GetUsername(database);
            string password = CreatePasswordHash();
            return new BasicAccData(username, password);
        }
        private static bool SignIn(string password)
        {
            while (true)
            {
                if (CheckOutPasswords(password)) return true;
                Console.WriteLine("Press 1 to try again or any other key to continue");
                string choice = Console.ReadLine();
                if (choice != "1") return false;
            }
        }
        
        private static string CreatePasswordHash()
        {
            Console.Write("Enter a password: ");
            string password = Console.ReadLine();

            // Generate a random salt
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            // Create the hash
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            // Concatenate the salt and hash
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            // Convert the salt and hash to a string
            string savedPasswordHash = Convert.ToBase64String(hashBytes);
            return savedPasswordHash;
        }
        private static bool CheckOutPasswords(string savedPasswordHash)
        {
            Console.Write("Enter a password to verify: ");
            string enteredPassword = Console.ReadLine();

            // Retrieve the stored salt and hash
            // retrieve the stored password hash
            byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            // Generate a new hash for the entered password
            var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, salt, 10000);
            byte[] newHash = pbkdf2.GetBytes(20);

            // Compare the new hash to the stored hash
            bool isPasswordValid = true;
            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != newHash[i])
                {
                    isPasswordValid = false;
                    break;
                }
            }
            Console.WriteLine($"<----Is password valid: {isPasswordValid}---->\n");
            return isPasswordValid;
        }
    }

    class BasicAccData
    {
        public string Name { get;  }
        public string Password { get;  }
        public BasicAccData(string name, string password)
        {
            Name = name;
            Password = password;
        }
    }
}