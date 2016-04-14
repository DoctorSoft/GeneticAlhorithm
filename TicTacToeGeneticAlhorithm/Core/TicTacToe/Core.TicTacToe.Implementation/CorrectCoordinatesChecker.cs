namespace Core.TicTacToe.Implementation
{
    using Core.TicTacToe.Constants;
    using Core.TicTacToe.Declaration;

    public class CorrectCoordinatesChecker : ICorrectCoordinatesChecker
    {
        public bool CoordinatesAreCorrect(int x, int y)
        {
            var result =
                !(x < 0 || x > GameFieldConstants.MaxCoordinate
                  || y < 0 || y > GameFieldConstants.MaxCoordinate);

            return result;
        }
    }
}
