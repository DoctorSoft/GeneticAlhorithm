namespace Core.MVC.PlayerVsPlayer.Models
{
    using Core.TicTacToe.Constants;

    public class PlayerVsPlayerNewGameCommandResult
    {
        public int GameId { get; set; }

        public CellCondition[,] GameField { get; set; }
    }
}
