namespace Core.TicTacToe.Declaration
{
    using Core.TicTacToe.Constants;
    using Core.TicTacToe.Models;

    public interface IGameProcessStatisticProvider
    {
        GameProcessStatistic GetGameProcessStatistic(CellCondition[,] gameField);
    }
}
