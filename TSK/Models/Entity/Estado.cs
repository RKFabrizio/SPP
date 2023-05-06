using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SPP.Models.Entity
{
    public partial class Estado
    {
        public Estado()
        {
            Pagos = new HashSet<Pago>();
        }

        public int IdEstado { get; set; }
        public string NombreEstado { get; set; }

        public virtual ICollection<Pago> Pagos { get; set; }
    }
}
