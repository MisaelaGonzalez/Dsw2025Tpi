namespace Dsw2025Tpi.Application.Exceptions
{
    [Serializable]
    public class EntityNotFoundException : Exception
    {
        //Se lanza cuando no se encuentra una entidad buscada
        //(por ejemplo, buscar un cliente por ID que no existe).
        public EntityNotFoundException()
        {
        }

        public EntityNotFoundException(string? message) : base(message)
        {
        }

        public EntityNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}