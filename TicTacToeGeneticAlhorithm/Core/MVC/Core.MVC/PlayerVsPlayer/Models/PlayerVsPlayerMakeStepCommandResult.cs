namespace Core.MVC.PlayerVsPlayer.Models
{
    using Core.TicTacToe.Constants;
    using Core.TicTacToe.Models;

    public class PlayerVsPlayerMakeStepCommandResult
    {
        public int GameId { get; set; }

        public CellCondition[,] GameField { get; set; }

        public int CellSize { get; set; }

        public GameProcessStatistic GameProcessStatistic { get; set; }
    }
}
