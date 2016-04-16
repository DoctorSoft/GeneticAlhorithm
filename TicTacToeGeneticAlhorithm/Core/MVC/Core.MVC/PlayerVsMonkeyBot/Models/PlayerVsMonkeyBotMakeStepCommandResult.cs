using Core.TicTacToe.Constants;
using Core.TicTacToe.Models;

namespace Core.MVC.PlayerVsMonkeyBot.Models
{
    public class PlayerVsMonkeyBotMakeStepCommandResult
    {
        public int GameId { get; set; }

        public CellCondition[,] GameField { get; set; }

        public int CellSize { get; set; }

        public GameProcessStatistic GameProcessStatistic { get; set; }
    }
}
