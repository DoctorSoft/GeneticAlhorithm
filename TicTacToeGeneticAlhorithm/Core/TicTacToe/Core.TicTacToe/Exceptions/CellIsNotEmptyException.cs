namespace Core.TicTacToe.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    public class CellIsNotEmptyException : Exception
    {
        public CellIsNotEmptyException()
        {
        }

        public CellIsNotEmptyException(string message) : base(message)
        {
        }

        public CellIsNotEmptyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CellIsNotEmptyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
