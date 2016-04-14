namespace Core.TicTacToe.Implementation
{
    using Core.TicTacToe.Constants;
    using Core.TicTacToe.Declaration;

    public class CorrectCoordinatesChecker : ICorrectCoordinatesChecker
    {
        public bool CoordinatesAreCorrect(int x, int y)
        {
            var result =
                !(x < GameFieldConstants.MinXCoordinate || x > GameFieldConstants.MaxXCoordinate
                  || y < GameFieldConstants.MinYCoordinate || y > GameFieldConstants.MaxYCoordinate);

            return result;
        }
    }
}
