using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Este código define un conjunto de modelos de datos inmutables (record) agrupados bajo
// la clase ProductModel. Se utilizan para representar productos dentro de una aplicación,
// muy probablemente en una API que maneja operaciones CRUD (crear, leer, actualizar, eliminar).

namespace Dsw2025Tpi.Application.Dtos

{
    public record ProductModel
    {
        //Request
        // Se utiliza cuando se quiere crear o actualizar un producto.
        public record ProductRequest(string Sku, string Name, decimal CurrentUnitPrice, string InternalCode, string Description, int StockQuantity);

        // Response
        // Se devuelve cuando se consulta un producto.
        public record ProductResponse(Guid Id, string Sku, string Name, decimal CurrentUnitPrice, string InternalCode, string Description, int StockQuantity);
        // Parecido a ProductResponse, pero incluye un campo extra:
        // IsActive: indica si el producto está activo o inactivo (por ejemplo,
        // si está disponible para la venta).
        public record ProductResponseUpdate(Guid Id, string Sku, string Name, decimal CurrentUnitPrice, string InternalCode, string Description, int StockQuantity, bool IsActive);
        //  para cuando solo se quiere devolver o trabajar con el ID del producto
        public record ProductResponseID(Guid Id);
    }
}
