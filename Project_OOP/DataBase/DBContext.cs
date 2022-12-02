using System.Collections.Generic;
using Project_OOP.GameItems;

namespace Project_OOP.DataBase
{
    public class DbContext
    {
        public List<GameAccount> UsersList { get; }
        public List<HistoryGame> GameHistory { get; }

        public DbContext()
        {
            UsersList = new List<GameAccount>();
            GameHistory = new List<HistoryGame>();
        }
    }
}