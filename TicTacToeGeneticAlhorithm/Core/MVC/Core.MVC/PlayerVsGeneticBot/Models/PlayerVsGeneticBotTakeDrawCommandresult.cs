using Core.TicTacToe.Constants;

namespace Core.MVC.PlayerVsGeneticBot.Models
{
    public class PlayerVsGeneticBotTakeDrawCommandResult
    {
        public CellCondition[,] GameField { get; set; }

        public int CellSize { get; set; }
    }
}
