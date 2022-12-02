using System;
namespace Project_OOP.GameItems
{
    public class GameAccount
    {
        public string UserName { get; }
        
        private int _currentRating;
        public int CurrentRating
        {
            get => _currentRating;
            set => _currentRating = value < 1 ? 1 : value;
        }
        private const int InitialRating = 100;

        public static readonly GameAccount System = new GameAccount();
        

        public GameAccount(string userName)
        {
            UserName = userName;
            CurrentRating = InitialRating;
        }

        private GameAccount()
        {
            CurrentRating = Int32.MaxValue - InitialRating;
            UserName = "System";
        }
        public virtual void WinGame(Game game, GameAccount opponent)
        {
            CurrentRating += game.Rating;
        }
        public virtual void LoseGame(Game game, GameAccount opponent)
        {
            CurrentRating -= game.Rating;
        }
        public void Draw(Game game, GameAccount opponent)
        {
            
        }
    }

    public class ThriftyGameAccount : GameAccount
    {
        public ThriftyGameAccount(string userName) : base(userName)
        {
        }
        public override void LoseGame(Game game, GameAccount opponent)
        {
            CurrentRating -= game.Rating / 2;
        }
    }

    public class PremiumGameAccount : GameAccount
    {
        public PremiumGameAccount(string userName) : base(userName)
        {
        }
        public override void WinGame(Game game, GameAccount opponent)
        {
            CurrentRating += game.Rating * 2;
        }
    }
}