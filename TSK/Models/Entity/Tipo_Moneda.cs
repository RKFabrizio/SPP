using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SPP.Models.Entity
{
    public partial class Tipo_Moneda
    {
        public Tipo_Moneda()
        {
            Pagos = new HashSet<Pago>();
        }

        public int IdTipoMoneda { get; set; }
        public string TipoMoneda { get; set; }
        public string Simbologia { get; set; }

        public virtual ICollection<Pago> Pagos { get; set; }
    }
}
