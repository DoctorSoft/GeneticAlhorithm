using Core.TicTacToe.Constants;

namespace Core.MVC.PlayerVsStatisticBot.Models
{
    public class PlayerVsStatisticBotNewGameCommandResult
    {
        public int GameId { get; set; }

        public CellCondition[,] GameField { get; set; }

        public int CellSize { get; set; }

        public bool IsBotTurn { get; set; }
    }
}
