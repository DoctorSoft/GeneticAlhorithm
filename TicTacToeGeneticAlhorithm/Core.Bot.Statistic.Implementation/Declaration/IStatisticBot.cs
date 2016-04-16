using Core.TicTacToe.Constants;
using Core.TicTacToe.Models;
using Data.Migration.Contexts;

namespace Core.Bot.Statistic.Implementation.Declaration
{
    public interface IStatisticBot
    {
        Coordinates GetStep(CellCondition[,] gameField, TicTacToeContext context);
    }
}
