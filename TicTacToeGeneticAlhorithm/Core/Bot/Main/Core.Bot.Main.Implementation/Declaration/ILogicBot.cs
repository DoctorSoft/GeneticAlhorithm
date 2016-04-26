namespace Core.Bot.Main.Implementation.Declaration
{
    using Core.TicTacToe.Constants;
    using Core.TicTacToe.Models;

    public interface ILogicBot
    {
        Coordinates GetStep(CellCondition[,] gameField);
    }
}
