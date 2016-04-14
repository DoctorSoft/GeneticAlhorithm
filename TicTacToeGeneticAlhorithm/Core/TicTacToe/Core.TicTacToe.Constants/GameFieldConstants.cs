namespace Core.TicTacToe.Constants
{
    public static class GameFieldConstants
    {
        public const int MinXCoordinate = 0;

        public const int MaxXCoordinate = 2;

        public const int XLength = MaxXCoordinate - MinXCoordinate + 1;

        public const int MinYCoordinate = 0;

        public const int MaxYCoordinate = 2;

        public const int YLength = MaxYCoordinate - MinYCoordinate + 1; 

        public const int CellsCount = XLength * YLength;

        public const int CellsCountToWin = 3;
    }
}
