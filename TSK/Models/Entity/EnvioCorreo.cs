using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SPP.Models.Entity
{
    public partial class EnvioCorreo
    {
        public int IdCorreo { get; set; }
        public string NombreCorreo { get; set; }
        public int IdPago { get; set; }
        public virtual Pago PagoDisNavigation { get; set; }

    }
}
