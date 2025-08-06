namespace Dsw2025Tpi.Application.Exceptions
{
   
    public class PriceNullException : Exception
    {
        // Probablemente se lanza cuando un producto no tiene precio asignado (precio nulo o inválido).
        public PriceNullException()
        {
        }

        public PriceNullException(string? message) : base(message)
        {
        }

        public PriceNullException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}