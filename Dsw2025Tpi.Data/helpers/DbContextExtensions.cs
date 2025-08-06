using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dsw2025Tpi.Data.helpers
{
    // Ese código define un método de extensión llamado Seedwork<T> que sirve para cargar datos iniciales
    // (seed data) en la base de datos desde un archivo .json.
    public static class DbContextExtensions
    {
        public static void Seedwork<T>(this Dsw2025TpiContext context, string dataSource) where T : class
        {
            if (context.Set<T>().Any()) return; // Si ya hay datos en la tabla de tipo T, sale sin hacer nada.
            var json = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, dataSource));//lee un archivo .json desde la ruta proporcionada:
            var entities = JsonSerializer.Deserialize<List<T>>(json, new JsonSerializerOptions
            { //Deserializa el contenido JSON en una lista de objetos de tipo T:
                PropertyNameCaseInsensitive = true,
            });
            if (entities == null || entities.Count == 0) return; //Verifica si no hay datos para insertar, y si es así, termina la ejecución del método
            context.Set<T>().AddRange(entities); //Los inserta todos en la tabla correspondiente
            context.SaveChanges();                  //y guarda los cambios.
        }
    }
}
