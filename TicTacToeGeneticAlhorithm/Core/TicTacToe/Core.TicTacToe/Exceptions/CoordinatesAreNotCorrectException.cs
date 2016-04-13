namespace Core.TicTacToe.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    public class CoordinatesAreNotCorrectException : Exception
    {
        public CoordinatesAreNotCorrectException()
        {
        }

        public CoordinatesAreNotCorrectException(string message) : base(message)
        {
        }

        public CoordinatesAreNotCorrectException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CoordinatesAreNotCorrectException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
