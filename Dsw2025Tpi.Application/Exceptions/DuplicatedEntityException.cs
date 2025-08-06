using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Application.Exceptions;

public class DuplicatedEntityException : Exception
{
    // Se lanza cuando se intenta agregar una entidad que
    // ya existe (por ejemplo, un producto con un SKU duplicado).
    public DuplicatedEntityException(string message) : base(message)
    {

    }
}
