using Data.Model.TicTacToe;

namespace Data.Model.StatisticBot
{
    public class FieldStatistic
    {
        public int FieldId { get; set; }

        public int Draws { get; set; }

        public int Wins { get; set; }

        public int Loses { get; set; }

        public Field Field { get; set; }
    }
}
