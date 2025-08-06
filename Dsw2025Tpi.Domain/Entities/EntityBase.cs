namespace Dsw2025Tpi.Domain.Entities;
// La clase es abstract: no se puede instanciar directamente, solo heredar.
//Se usa como una base común para todas las entidades del dominio
public abstract class EntityBase
{   //El constructor es protected, lo que significa que solo las clases hijas pueden llamarlo.
    //Se asigna un nuevo Guid como identificador único (Id) al crear la instancia.
    protected EntityBase()
    {
        Id = Guid.NewGuid();
    }
    public Guid Id { get; protected set; }
}

