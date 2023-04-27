using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SPP.Models.Entity
{
    public partial class Tipo_Cuenta
    {
        public Tipo_Cuenta()
        {
            Proveedores = new HashSet<Proveedor>();
        }

        public int IdTipoCuenta { get; set; }
        public string TipoCuenta { get; set; }

        public virtual ICollection<Proveedor> Proveedores { get; set; }
    }

}
