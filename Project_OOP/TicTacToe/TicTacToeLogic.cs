using System;
using System.Collections.Generic;

namespace Project_OOP.TicTacToe
{
    public class TicTacToeLogic
    {
        public  List<int> AllMoves { get; }
        public  List<int> Corners { get; }

        public char[,] KeyMatrix { get; }
        public char[,] Playground { get; private set; }
        
        public TicTacToeLogic()
        {
            KeyMatrix = new char[,]
            {
                {' ', ' ', ' '},
                {' ', ' ', ' '},
                {' ', ' ', ' '},
            };
            AllMoves = new List<int>{1,2,3,4,5,6,7,8,9};
            Corners = new List<int> { 1, 3, 7, 9 };
            Playground = new char[,]
            {
                {' ', ' ', '|', ' ', ' ', ' ', '|', ' ', ' '},
                {'-', '-', '+', '-', '-', '-', '+', '-', '-'},
                {' ', ' ', '|', ' ', ' ', ' ', '|', ' ', ' '},
                {'-' ,'-', '+', '-', '-', '-', '+', '-', '-'},
                {' ', ' ', '|', ' ', ' ', ' ', '|', ' ', ' '},
            };
        }
        public bool MakeMove(char c)
        {
            Console.Write("--> ");
            int position;
            try
            {
                position = Convert.ToInt32(Console.ReadLine());
            }
            catch (FormatException e)
            {
                Console.WriteLine("You can type only numbers (1 - 9)");
                Console.WriteLine(e.ToString());
                return false;
            }

            if (position < 1 || position > 9)
            {
                Console.WriteLine("Choose only from  1 - 9");
                return false;
            }

            if (!AllMoves.Contains(position))
            {
                Console.WriteLine("Position is already engaged!");
                return false;
            }
            SetPosWithAllAssignments(position, c);
            return true;
        }
        private bool Engaged(int row, int col)
        {
            return Playground[row, col] != ' ';
        }
        private void SetPosition(int row, int col, char c)
        {
            if (Engaged(row, col)) Console.WriteLine("Position is already engaged!");
            else Playground[row, col] = c;
        }

        public void SetPosWithAllAssignments(int pos, char c)
        {
            AllMoves.Remove(pos);
            Corners.Remove(pos);
            ChooseAndSetPosition(pos, c);
            ChangeKeyMatrix(pos, c);
        }

        private void ChooseAndSetPosition(int position, char c)
        {
            switch (position)
            {
                case 1:
                    SetPosition(0,0, c);
                    break;
                case 2:
                    SetPosition(0,4, c);
                    break;
                case 3:
                    SetPosition(0,8, c);
                    break;
                case 4:
                    SetPosition(2,0, c);
                    break;
                case 5:
                    SetPosition(2,4, c);
                    break;
                case 6:
                    SetPosition(2,8, c);
                    break;
                case 7:
                    SetPosition(4,0, c);
                    break;
                case 8:
                    SetPosition(4,4, c);
                    break;
                case 9:
                    SetPosition(4,8, c);
                    break;
                default: 
                    Console.WriteLine("Choose only from  1 - 9");
                    break;
            }
        }
        
        private void ChangeKeyMatrix(int pos, char c)
        {
            int count = 0;
            for (int i = 0; i < KeyMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < KeyMatrix.GetLength(1); j++)
                {
                    count++;
                    if (count != pos) continue;
                    KeyMatrix[i, j] = c;
                    return;
                }
            }
        }
        public bool WinCombination(char c)
        {
            return Playground[0, 0] == c && Playground[0, 4] == c && Playground[0, 8] == c ||
                   Playground[2, 0] == c && Playground[2, 4] == c && Playground[2, 8] == c ||
                   Playground[4, 0] == c && Playground[4, 4] == c && Playground[4, 8] == c ||
                   Playground[0, 0] == c && Playground[2, 0] == c && Playground[4, 0] == c ||
                   Playground[0, 4] == c && Playground[2, 4] == c && Playground[4, 4] == c ||
                   Playground[0, 8] == c && Playground[2, 8] == c && Playground[4, 8] == c ||
                   Playground[0, 0] == c && Playground[2, 4] == c && Playground[4, 8] == c ||
                   Playground[0, 8] == c && Playground[2, 4] == c && Playground[4, 0] == c ;
        }

        public bool DrawCombination()
        {
            for (int i = 0; i < KeyMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < KeyMatrix.GetLength(1); j++)
                {
                    if (KeyMatrix[i, j] == ' ') return false;
                }
            }
            return true;
        }
    
    }
}