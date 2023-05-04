using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SPP.Models.Entity
{
    public partial class Tipo_Pago
    {
        public Tipo_Pago()
        {
            Pagos = new HashSet<Pago>();
        }

        public int IdTipoPago { get; set; }
        public string TipoPago { get; set; }

        public virtual ICollection<Pago> Pagos { get; set; }
    }
}
