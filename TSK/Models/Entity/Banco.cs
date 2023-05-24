using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SPP.Models.Entity
{
    public partial class Banco
    {
        public Banco()
        {
            Pagos = new HashSet<Pago>();
        }

        public int IdBanco { get; set; }
        public string NombreBanco { get; set; }

        public virtual ICollection<Pago> Pagos { get; set; }
    }
}
