using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace SPP.Models.Entity
{
    public class Tipo_Documento
    {
        public Tipo_Documento()
        {
            Proveedores = new HashSet<Proveedor>();
        }

        public int IdTipoDocumento { get; set; }
        public string NombreDocumento { get; set; }

        public virtual ICollection<Proveedor> Proveedores { get; set; }
    }
}
