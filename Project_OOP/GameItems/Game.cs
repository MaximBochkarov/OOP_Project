using System;
using Project_OOP.TicTacToe;

namespace Project_OOP.GameItems
{
    public abstract class Game
    {
        private static int _gameIndexSeed = 38256;
        private readonly GameAccount _acc1, _acc2;
        protected int GameIndex { get; set; }

        protected readonly int rating;
        public abstract int Rating { get; }
        protected bool AiGame { get; set; }
        public HistoryGame HistoryGame { get; private set; }
        protected Game(GameAccount acc1, GameAccount acc2, int rating)
        {
            _acc1 = acc1;
            _acc2 = acc2;
            this.rating = rating;
        }
        protected Game(GameAccount acc1, GameAccount acc2)
        {
            _acc1 = acc1;
            _acc2 = acc2;
        }
        protected static int GetNextId() => ++_gameIndexSeed;
        protected void Play()
        {
            try
            {
                CheckNegativeRating(Rating);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine("Exception caught starting game with negative rating");
                Console.WriteLine(e.ToString());
                return;
            }
            if (_acc1.CurrentRating - Rating < 1 || _acc2.CurrentRating - Rating < 1)
            {
                Console.WriteLine("Game rating is too high. Player`s rating can not be lower than 1");
                return;
            }

            Console.WriteLine("..........G..a..m..e....S..t..a..r..t..e..d..........");
            Console.WriteLine($"Game index: {GameIndex}");
            var ticTacToeGame = new TicTacToeGame();
            int decide = ticTacToeGame.StartGame(AiGame);

            GameStatus gameStatus;
            
            switch (decide)
            {
                case 0:
                    _acc1.Draw(this, _acc2);
                    _acc2.Draw(this, _acc1);
                    gameStatus = GameStatus.Draw;
                    break;
                case 1:
                    AssignStatusWinner(_acc1, _acc2);
                    gameStatus = GameStatus.Win;
                    break;
                case 2:
                    AssignStatusWinner(_acc2, _acc1);
                    gameStatus = GameStatus.Lose;
                    break;
                default: 
                    gameStatus = GameStatus.Unknown;
                    Console.WriteLine("Error in Game decide section");
                    break;
            }
            // Program.DbContext.GameHistory.Add(new HistoryGame(_acc1, _acc2, Rating, GameIndex, gameStatus, GetType().Name));
            HistoryGame = new HistoryGame(_acc1, _acc2, Rating, GameIndex, gameStatus, GetType().Name);
        }
        private void AssignStatusWinner(GameAccount winner, GameAccount looser)
        {
            winner.WinGame(this, looser);
            looser.LoseGame(this, winner);
        }
        private static void CheckNegativeRating(int rating)
        {
            if(rating < 0)
                throw new ArgumentOutOfRangeException(nameof(rating),"Amount of rating must be positive");
        }
    }
    
    public class StandardGame : Game
    {
        public StandardGame(GameAccount acc1, GameAccount acc2, int rating) : base(acc1, acc2, rating)
        {
            GameIndex = GetNextId();
            Play();
        }
        public override int Rating => rating;
        
    }

    public class PracticeGame : Game
    {
        public PracticeGame(GameAccount acc1, GameAccount acc2) : base(acc1, acc2)
        {
            GameIndex = GetNextId();
            Play();
        }
        public override int Rating => 0;
        
    }

    public class SoloRankedGame : Game
    {
        public SoloRankedGame(GameAccount acc1, int rating) : base(acc1, GameAccount.System ,rating)
        {
            GameIndex = GetNextId();
            AiGame = true;
            Play();
        }
        public override int Rating => rating;
    }
}