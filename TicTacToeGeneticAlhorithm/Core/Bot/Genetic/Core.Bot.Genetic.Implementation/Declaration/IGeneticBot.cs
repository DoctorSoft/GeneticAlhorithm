using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Bot.Genetic.Implementation.Declaration
{
    using Core.TicTacToe.Constants;
    using Core.TicTacToe.Models;

    using Data.Migration.Contexts;

    public interface IGeneticBot
    {
        Coordinates GetStep(CellCondition[,] gameField, TicTacToeContext context);
    }
}
