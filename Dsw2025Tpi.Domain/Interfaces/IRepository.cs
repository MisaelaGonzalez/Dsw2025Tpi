using Dsw2025Tpi.Domain.Entities;
using System.Linq.Expressions;

namespace Dsw2025Tpi.Domain.Interfaces;

// ESTAS FUNCIONES SON ASINCRONAS Y GENERICOS

// <T> T tiene que ser cualquier tipo que herede de EntityBase
public interface IRepository
{   // Es una interfaz genérica que sirve para definir operaciones comunes
    // que se pueden realizar sobre cualquier entidad que herede de EntityBase.
    // Todas las entidades (Customer, Order, Product, etc.) heredan de EntityBase,
    // así que se pueden usar con este repositorio.
    Task<T?> GetById<T>(Guid id, params string[] include) where T : EntityBase;
    // Devuelve una entidad por su Id.
    // params string[] include: permite incluir propiedades relacionadas(navegación).
    // Ejemplo: "OrderItems", "Customer".
    Task<IEnumerable<T>?> GetAll<T>(params string[] include) where T : EntityBase;
    //Devuelve todos los objetos del tipo T.
    Task<T?> First<T>(Expression<Func<T, bool>> predicate, params string[] include) where T : EntityBase;
    //Devuelve el primer elemento que cumpla esa condición, o null si no hay.
    Task<IEnumerable<T>?> GetFiltered<T>(Expression<Func<T, bool>> predicate, params string[] include) where T : EntityBase;
    Task<T> Add<T>(T entity) where T : EntityBase;
    //Agrega una entidad a la base de datos.
    Task<T> Update<T>(T entity) where T : EntityBase;
    Task<T> Delete<T>(T entity) where T : EntityBase;
    Task<IEnumerable<T>> GetAll<T>(string includeProperties = "") where T : class;

}
