using System.Collections.Generic;
using Data.Model.TicTacToe;

namespace Data.Model.GeneticBot
{
    public class GeneticIndividual
    {
        public int GeneticIndividualId { get; set; }

        public int GenerationNumber { get; set; }

        public int Draws { get; set; }

        public int Wins { get; set; }

        public int Loses { get; set; }

        public ICollection<GeneticFactor> GeneticFactors { get; set; }
    }
}
