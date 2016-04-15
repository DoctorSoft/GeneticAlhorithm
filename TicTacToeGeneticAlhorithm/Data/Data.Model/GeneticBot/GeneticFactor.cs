using System.Collections.Generic;
using Data.Model.TicTacToe;

namespace Data.Model.GeneticBot
{
    public class GeneticFactor
    {
        public int GeneticFactorId { get; set; }

        public int Factor { get; set; }

        public int FieldId { get; set; }

        public Field Field { get; set; }

        public GeneticIndividual GeneticIndividual { get; set; }
    }
}
