using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUIBlazorAzureFunctionCRUDCosmosDB.Models
{
    public class TodoItem
    {
        public string? id { get; set; }  // ID único del ítem
        public string? name { get; set; }  // Nombre del ítem
        public string? description { get; set; }  // Descripción del ítem
        public bool IsCompleted { get; set; }  // Estado de completado (si es necesario)
        public string? _rid { get; set; }  // Metadatos específicos de Cosmos DB
        public string? _self { get; set; }
        public string? _etag { get; set; }
        public string? _attachments { get; set; }
        public long? _ts { get; set; }  // Timestamp de creación o actualización
    }
}
