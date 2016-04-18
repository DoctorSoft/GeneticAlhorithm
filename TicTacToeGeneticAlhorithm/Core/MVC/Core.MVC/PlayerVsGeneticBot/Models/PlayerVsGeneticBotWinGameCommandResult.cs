using System.Collections.Generic;
using Core.TicTacToe.Constants;
using Core.TicTacToe.Models;

namespace Core.MVC.PlayerVsGeneticBot.Models
{
    public class PlayerVsGeneticBotWinGameCommandResult
    {
        public CellCondition[,] GameField { get; set; }

        public int CellSize { get; set; }

        public List<Coordinates> WinCoordinates { get; set; }

        public string Message { get; set; }
    }
}
