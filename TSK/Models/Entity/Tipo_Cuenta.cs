using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SPP.Models.Entity
{
    public partial class Tipo_Cuenta
    {
        public Tipo_Cuenta()
        {
            Pagos = new HashSet<Pago>();
        }

        public int IdTipoCuenta { get; set; }
        public string TipoCuenta { get; set; }

        public virtual ICollection<Pago> Pagos { get; set; }
    }

}
