namespace Data.Model.TicTacToe
{
    public class Game
    {
        public int GameId { get; set; }

        public int FieldNumber { get; set; }

        public Field Field { get; set; }
    }
}
