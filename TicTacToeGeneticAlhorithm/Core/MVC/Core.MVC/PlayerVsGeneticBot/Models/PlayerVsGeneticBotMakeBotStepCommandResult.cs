using Core.TicTacToe.Constants;
using Core.TicTacToe.Models;

namespace Core.MVC.PlayerVsGeneticBot.Models
{
    public class PlayerVsGeneticBotMakeBotStepCommandResult
    {
        public int GameId { get; set; }

        public CellCondition[,] GameField { get; set; }

        public int CellSize { get; set; }

        public Coordinates BotStep { get; set; }

        public GameProcessStatistic GameProcessStatistic { get; set; }
    }
}
