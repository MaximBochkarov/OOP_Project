using System;
using System.Collections.Generic;
using Project_OOP.GameItems;

namespace Project_OOP.DataBase
{
    public class DBContext
    {
        public List<GameAccount> UsersList { get; }
        public List<HistoryGame> GameHistory { get; }

        public DBContext()
        {
            UsersList = new List<GameAccount>();
            GameHistory = new List<HistoryGame>();
        }
    }
}