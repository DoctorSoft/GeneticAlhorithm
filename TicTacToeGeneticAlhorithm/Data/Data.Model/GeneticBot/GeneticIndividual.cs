using System.Collections.Generic;
using Data.Model.TicTacToe;

namespace Data.Model.GeneticBot
{
    public class GeneticIndividual
    {
        public int GeneticIndividualId { get; set; }

        public int GenerationNumber { get; set; }

        public int ImportanceOrder { get; set; }

        public double Score { get; set; }

        public int PlayedGames { get; set; }

        public ICollection<GeneticFactor> GeneticFactors { get; set; }
    }
}
