using System;

namespace Project_OOP.TicTacToe
{
    public class TicTacToeGame
    {
        private bool _isFinished;

        private bool _turn;

        private readonly TicTacToeLogic _gameLogic;

        private readonly TicTacToeBot _bot;

        public TicTacToeGame()
        {
            _isFinished = false;
            _turn = true;
            _gameLogic = new TicTacToeLogic();
            _bot = new TicTacToeBot(_gameLogic);
        }

        public int StartGame(bool aiGame)
        {
            char sign = TicTacToeAdditions.ChooseSign();
            _turn = TicTacToeAdditions.ChooseOrder();
            while (!_isFinished)
            {
                int result;
                Draw(_gameLogic.Playground);

                if (_turn)
                {
                    if (!_gameLogic.MakeMove(sign)) continue;
                    result = CheckCombinations(sign);
                }
                else
                {
                    if (aiGame)
                    {
                        _bot.BotMakeMove(TicTacToeAdditions.OppositeSign(sign));
                    }
                    else
                    {
                        if (!_gameLogic.MakeMove(TicTacToeAdditions.OppositeSign(sign))) continue;
                    }
                    result = CheckCombinations(TicTacToeAdditions.OppositeSign(sign));
                }

                if (result != -1)
                {
                    Draw(_gameLogic.Playground);
                    return result;
                }

                _turn = !_turn;
            }
            return -1;
        }
        private int CheckCombinations(char c)
        {
            _isFinished = _gameLogic.WinCombination(c);
            if (_isFinished)
            {
                return _turn ? 1 : 2;
            }

            if (_gameLogic.DrawCombination())
            {
                return 0;
            }

            return -1;
        }

        private static void Draw(char[,] arr)
        {
            ShowPlayground(arr);
        }

        private static void ShowPlayground(char[,] arr)
        {
            Console.WriteLine("\n");
            for (var i = 0; i < arr.GetLength(0); i++)
            {
                for (var j = 0; j < arr.GetLength(1); j++)
                {
                    Console.Write(arr[i, j]);
                }

                Console.WriteLine("");
            }

            Console.WriteLine();
        }
    }
}