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
        public string CuentaBancaria { get; set; }
        public string CCI { get; set; }
        public string BeneficiarioNombre { get; set; }
        public string BeneficiarioDni { get; set; }
        public int IdBanco { get; set; }
        public int IdTipoCuenta { get; set; }
        public int IdPais { get; set; }

        public virtual Banco IdDisBanco { get; set; }
        public virtual Tipo_Cuenta IdDisTipoCuenta { get; set; }
        public virtual Pais IdDisPais { get; set; }
        
        public virtual ICollection<Pago> Pagos { get; set; }
    }
}

