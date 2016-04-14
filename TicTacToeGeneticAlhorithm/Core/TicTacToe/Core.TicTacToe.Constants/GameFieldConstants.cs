namespace Core.TicTacToe.Constants
{
    public static class GameFieldConstants
    {
        public const int MaxCoordinate = 2;

        public const int LineLength = MaxCoordinate - MaxCoordinate + 1;

        public const int CellsCount = LineLength * LineLength;

        public const int CellsCountToWin = 3;
    }
}
