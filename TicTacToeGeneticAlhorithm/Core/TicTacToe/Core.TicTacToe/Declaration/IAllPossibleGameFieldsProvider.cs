namespace Core.TicTacToe.Declaration
{
    using System.Collections.Generic;

    using Core.TicTacToe.Models;

    public interface IAllPossibleGameFieldsProvider
    {
        List<GameFieldCombination> GenerateAllPossibleGameFieldCombanations();
    }
}
