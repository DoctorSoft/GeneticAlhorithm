using Core.TicTacToe.Constants;

namespace Core.MVC.PlayerVsMonkeyBot.Models
{
    public class PlayerVsMonkeyBotTakeDrawCommandResult
    {
        public CellCondition[,] GameField { get; set; }

        public int CellSize { get; set; }
    }
}
