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
        
        public string AccType { get; set; }

        public GameAccount(string userName)
        {
            // Program.DbContext.UsersList.Add(this);
            UserName = userName;
            CurrentRating = InitialRating;
            AccType = "DefaultAccount";
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
            AccType = "ThriftyGameAccount";
        }
        public override void LoseGame(Game game, GameAccount opponent)
        {
            CurrentRating -= game.Rating / 2;
        }
    }

    public class ExtraSeriesPointsGameAccount : GameAccount
    {
        private int _series;
        public ExtraSeriesPointsGameAccount(string userName) : base(userName)
        {
            _series = 0;
            AccType = "ExtraSeriesPointsGameAccount";
        }
        public override void WinGame(Game game, GameAccount opponent)
        {
            SeriesCount();
            SeriesExtraPoints();
            CurrentRating += game.Rating;
        }
        public override void LoseGame(Game game, GameAccount opponent)
        {
            SeriesReset();
            CurrentRating -= game.Rating;
        }
        private void SeriesExtraPoints()
        {
            if (_series < 3 || _series > 10) return;

            switch (_series)
            {
                case 3:
                    CurrentRating += 30;
                    break;
                case 5:
                    CurrentRating += 50;
                    break;
                case 10:
                    CurrentRating += 100;
                    break;
            }
        }
        private void SeriesCount() => ++_series;
        private void SeriesReset() => _series = 0;
    }
}