using System;
using Project_OOP.DataBase;
using Project_OOP.GameItems;

namespace Project_OOP
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            GameProcess gameProcess = new GameProcess();
            gameProcess.Start();
        }
    }
}