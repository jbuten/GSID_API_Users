using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Users
{

    using MongoDB.Bson.Serialization.Attributes;
    using System.ComponentModel.DataAnnotations;

    public class UserRol
    {
        [BsonId]
        public string Id { get; set; } = DateTime.Now.Ticks.ToString();
        [Required(ErrorMessage = "Seleccione una aplicacion")]
        public string App { get; set; } = String.Empty;
        public string AppName { get; set; } = String.Empty;
        [Required(ErrorMessage = "Seleccione un rol")]
        public string Rol { get; set; } = String.Empty;
        public string RolName { get; set; } = String.Empty;
        public DateTime AddDate { get; set; }
        public string AddBy { get; set; } = String.Empty;
        public DateTime DeleteDate { get; set; }
        public string DeleteBy { get; set; } = String.Empty;
        public bool Inactive { get; set; }


    }
}
