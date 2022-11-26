using System;
using System.Threading;

namespace Project_OOP.TicTacToe
{
    public class TicTacToeBot
    {
        private static Random _rand;
        private readonly TicTacToeLogic _logic;
        private bool _once = true;

        public TicTacToeBot(TicTacToeLogic logic)
        {
            _rand = new Random();
            _logic = logic;
        }
        public void BotMakeMove(char c)
        {
            int position;
            Thread.Sleep(1000);
            var fillRowCol = FillThirdRowCol(c);
            var fillDiagonal = FillThirdDiagonal(c);
            if (fillRowCol != -1)
            {
                position = fillRowCol;
                
            }
            else if (fillDiagonal != -1)
            {
                position = fillDiagonal;
            }
            else if (_once)
            {
                position = GetRandomCornerPos();
                _once = false;
            }
            else if (_logic.AllMoves.Contains(5))
            {
                position = 5;
            }
            else
            {
                position = GetRandomPos();
            }
            _logic.SetPosWithAllAssignments(position, c);
        }

        private int FillThirdRowCol(char c)
        {
            int mainPosition = -1;
            int secondaryPosition = -1;
            for (var i = 0; i < _logic.KeyMatrix.GetLength(0); i++)
            {
                char[] row = new char[3];
                char[] col = new char[3];
                for (var j = 0; j < _logic.KeyMatrix.GetLength(1); j++)
                {
                    row[j] = _logic.KeyMatrix[i, j];
                    col[j] = _logic.KeyMatrix[j, i];
                }
                // Check your options
                var rowResult1 = GetCheckedRowPos(row, i, c); 
                if (rowResult1 != -1) mainPosition = rowResult1;
                var colResult1 = GetCheckedColPos(col, i, c);
                if (colResult1 != -1) mainPosition = colResult1;

                // Check opponent`s options
                var rowResult2 = GetCheckedRowPos(row, i, TicTacToeAdditions.OppositeSign(c)); 
                if (rowResult2 != -1) secondaryPosition = rowResult2;
                var colResult2 = GetCheckedColPos(col, i, TicTacToeAdditions.OppositeSign(c));
                if (colResult2 != -1) secondaryPosition = colResult2;
            }
            return mainPosition != -1 ? mainPosition : secondaryPosition;
        }
        
        private int GetCheckedRowPos(char[] row, int rowPos, char c)
        {
            var fill = TicTacToeAdditions.CheckPosition(row, c);
            if (fill != -1) return fill + rowPos * 3;
            return -1;
        }
        private int GetCheckedColPos(char[] col, int colPos, char c)
        {
            var fill = TicTacToeAdditions.CheckPosition(col, c);
            if (fill != -1) return colPos+1 + (fill-1) * 3;
            return -1;
        }
        private int FillThirdDiagonal(char c)
        {
            char[] mainDiagonal = new char[3];
            char[] sideDiagonal = new char[3];
            int count = 0;
            
            for (int i = 0; i < _logic.KeyMatrix.GetLength(0); i++)
            {
                mainDiagonal[count] = _logic.KeyMatrix[i, i];
                count++;
            }
            count = 0;
            int j = 0;
            for (int i = 2; i >= 0; i--)
            {
                sideDiagonal[count] = _logic.KeyMatrix[i, j];
                count++;
                j++;
            }
            var mainDiagonalResult = GetCheckedMainDiagonalPos(mainDiagonal, c);
            if (mainDiagonalResult != -1) return mainDiagonalResult;
            
            var sideDiagonalResult = GetCheckedSideDiagonalPos(sideDiagonal, c);
            if (sideDiagonalResult != -1) return sideDiagonalResult;
            
            return -1;
        }
        private int GetCheckedMainDiagonalPos(char[] row, char c)
        {
            var fillMy = TicTacToeAdditions.CheckPosition(row, c);
            var fillHis = TicTacToeAdditions.CheckPosition(row, TicTacToeAdditions.OppositeSign(c));
            if (fillMy != -1) return fillMy + (fillMy-1) * 3;
            if (fillHis != -1) return fillHis + (fillHis-1) * 3;
            return -1;
        }
        private int GetCheckedSideDiagonalPos(char[] row, char c)
        {
            var fillMy = TicTacToeAdditions.CheckPosition(row, c);
            var fillHis = TicTacToeAdditions.CheckPosition(row, TicTacToeAdditions.OppositeSign(c));
            if (fillMy != -1)
            {
                int getMyCof = 2;
                getMyCof = fillMy > 2 ? fillMy - 2 : fillMy + 2;
                return fillMy + (getMyCof-1) * 3;
            }
            if (fillHis != -1)
            {
                int getHisCof = 2;
                getHisCof = fillHis > 2 ? fillHis - 2 : fillHis + 2;
                return fillHis + (getHisCof-1) * 3;
            }
            return -1;
        }
        
        private int GetRandomCornerPos()
        {
            int position;
            while (true)
            {
                position = _rand.Next(1, 10);
                if (!_logic.Corners.Contains(position)) continue;
                _logic.Corners.Remove(position);
                break;
            }
            return position;
        }
        private int GetRandomPos()
        {
            int position;
            while (true)
            {
                position = _rand.Next(1, 10);
                if (!_logic.AllMoves.Contains(position)) continue;
                break;
            }
            return position;
        }
    }
}