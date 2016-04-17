using Core.TicTacToe.Constants;

namespace Core.MVC.PlayerVsStatisticBot.Models
{
    public class PlayerVsStatisticBotTakeDrawCommandResult
    {
        public CellCondition[,] GameField { get; set; }

        public int CellSize { get; set; }
    }
}
