using System.Collections.Generic;
using Data.Model.GeneticBot;
using Data.Model.StatisticBot;

namespace Data.Model.TicTacToe
{
    public class Field
    {
        public int FieldId { get; set; }

        public string FirstVariant { get; set; }

        public string SecondVariant { get; set; }

        public string ThirdVariant { get; set; }

        public string FourthVariant { get; set; }

        public FieldStatistic FieldStatistic { get; set; }

        public ICollection<Game> Games { get; set; }

        public ICollection<GeneticFactor> GeneticFactors { get; set; }
    }
}
