using Core.TicTacToe.Constants;
using Core.TicTacToe.Models;

namespace Core.Bot.Main.Implementation.Declaration
{
    public interface IMonkeyBot
    {
        Coordinates GetStep(CellCondition[,] gameField);
    }
}
