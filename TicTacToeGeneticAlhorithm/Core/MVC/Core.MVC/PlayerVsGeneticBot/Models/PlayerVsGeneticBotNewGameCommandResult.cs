using Core.TicTacToe.Constants;

namespace Core.MVC.PlayerVsGeneticBot.Models
{
    public class PlayerVsGeneticBotNewGameCommandResult
    {
        public int GameId { get; set; }

        public CellCondition[,] GameField { get; set; }

        public int CellSize { get; set; }

        public bool IsBotTurn { get; set; }
    }
}
