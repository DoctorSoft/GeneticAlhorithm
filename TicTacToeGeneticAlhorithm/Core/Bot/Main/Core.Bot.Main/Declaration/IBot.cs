using Core.TicTacToe.Constants;
using Core.TicTacToe.Models;

namespace Core.Bot.Main.Declaration
{
    public interface IBot
    {
        Coordinates GetStep(CellCondition[,] gameField);
    }
}
