using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

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
        public virtual void Draw(Game game, GameAccount opponent)
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
    public class ExtraSeriesPointsGameAccount : GameAccount
    {
        [JsonRequired]
        private readonly List<GameStatus> _matchResults;
        [JsonRequired]
        private int _skip;
        [JsonRequired]
        private bool _hasThree;
        [JsonRequired]
        private bool _hasFive;
        public ExtraSeriesPointsGameAccount(string userName) : base(userName)
        {
            _matchResults = new List<GameStatus>();
            _skip = 0;
            _hasThree = false;
            _hasFive = false;
        }
        
        public override void WinGame(Game game, GameAccount opponent)
        {
            CurrentRating += game.Rating;
            _matchResults.Add(GameStatus.Win);
            CheckSeries();
        }
        public override void LoseGame(Game game, GameAccount opponent)
        {
            CurrentRating -= game.Rating;
            _matchResults.Add(GameStatus.Lose);
        }
        public override void Draw(Game game, GameAccount opponent)
        {
            _matchResults.Add(GameStatus.Draw);
        }

        private void CheckSeries()
        {
            int series = 0;
            for (int i = _skip; i < _matchResults.Count; i++)
            {
                if (_matchResults.ElementAt(i) == GameStatus.Win)
                {
                    series++;
                    switch (series)
                    {
                        case 3 when _hasThree:
                            break;
                        case 5 when _hasFive:
                            break;
                        default:
                            ChargeSeriesExtraPoints(series);
                            break;
                    }
                }
                else
                {
                    _hasThree = false;
                    _hasFive = false;
                    _skip += series + 1;
                    series = 0;
                }
            }
        }
        private void ChargeSeriesExtraPoints(int series)
        {
            if (series < 3 || series > 10) return;

            switch (series)
            {
                case 3:
                    CurrentRating += 30;
                    _hasThree = true;
                    break;
                case 5:
                    CurrentRating += 50;
                    _hasFive = true; 
                    break;
                case 10:
                    CurrentRating += 100;
                    break;
            }
        }
    }
}