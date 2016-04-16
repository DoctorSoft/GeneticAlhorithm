using System.Collections.Generic;
using Core.TicTacToe.Constants;
using Core.TicTacToe.Models;

namespace Core.MVC.PlayerVsMonkeyBot.Models
{
    public class PlayerVsMonkeyBotWinGameCommandResult
    {
        public CellCondition[,] GameField { get; set; }

        public int CellSize { get; set; }

        public List<Coordinates> WinCoordinates { get; set; } 
    }
}
