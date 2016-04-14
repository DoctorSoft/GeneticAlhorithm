namespace Core.TicTacToe.Models
{
    using Core.TicTacToe.Constants;

    public class GameProcessStatistic
    {
        public GameStatus GameStatus { get; set; }

        public WinStatistic WinStatistic { get; set; }
    }
}
