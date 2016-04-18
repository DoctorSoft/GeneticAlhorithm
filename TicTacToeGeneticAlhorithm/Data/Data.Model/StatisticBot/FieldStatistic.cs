using Data.Model.TicTacToe;

namespace Data.Model.StatisticBot
{
    public class FieldStatistic
    {
        public int FieldId { get; set; }

        public double Score { get; set; }

        public int PlayedGames { get; set; }

        public Field Field { get; set; }
    }
}
