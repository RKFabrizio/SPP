using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SPP.Models.Entity
{
    public partial class Proveedor
    {
        public Proveedor()
        {
            Pagos = new HashSet<Pago>();
        }

        public int IdProveedor { get; set; }
        public string NombreProveedor { get; set; }
        public int IdTipoDoc { get; set; }
        public string Numero_Doc { get; set; }
        public int IdPais { get; set; }

        public virtual Tipo_Documento IdDisDocumento { get; set; }
        public virtual Pais IdDisPais { get; set; }
        
        public virtual ICollection<Pago> Pagos { get; set; }
    }
}

