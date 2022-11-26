namespace Project_OOP.GameItems
{
    public class HistoryGame
    {
        public int Rating { get; }
        public GameAccount Opponent { get; }
        public GameAccount CurrentAccount { get; }
        public int GameIndex { get; }
        public GameStatus GameStatus{ get; }
        public string GameName { get; }

        public HistoryGame(GameAccount currentAccount, GameAccount opponent, int rating, int gameIndex, GameStatus gameStatus, string gameName)
        {
            CurrentAccount = currentAccount;
            Opponent = opponent;
            Rating = rating;
            GameIndex = gameIndex;
            GameStatus = gameStatus;
            GameName = gameName;
        }
        
    }
}