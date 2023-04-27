using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SPP.Models.Entity
{
    public partial class Tipo_Adelanto
    {
        public Tipo_Adelanto()
        {
            Pagos = new HashSet<Pago>();
        }

        public int IdTipoAdelanto { get; set; }
        public string TipoAdelanto { get; set; }

        public virtual ICollection<Pago> Pagos { get; set; }
    }
}
