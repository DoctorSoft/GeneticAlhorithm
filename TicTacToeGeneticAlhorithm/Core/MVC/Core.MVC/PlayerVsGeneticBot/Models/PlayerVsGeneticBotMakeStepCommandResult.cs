using Core.TicTacToe.Constants;
using Core.TicTacToe.Models;

namespace Core.MVC.PlayerVsGeneticBot.Models
{
    public class PlayerVsGeneticBotMakeStepCommandResult
    {
        public int GameId { get; set; }

        public CellCondition[,] GameField { get; set; }

        public int CellSize { get; set; }

        public GameProcessStatistic GameProcessStatistic { get; set; }
    }
}
