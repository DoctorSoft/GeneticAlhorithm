using Core.TicTacToe.Constants;

namespace Core.MVC.PlayerVsPlayer.Models
{
    public class PlayerVsPlayerTakeDrawCommandResult
    {
        public CellCondition[,] GameField { get; set; }

        public int CellSize { get; set; }
    }
}
