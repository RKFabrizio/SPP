using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SPP.Models.Entity
{
    public partial class Pais
    {
        public Pais()
        {
            Proveedores = new HashSet<Proveedor>();
            Companias = new HashSet<Compania>();
        }

        public int IdPais { get; set; }
        public string NombrePais { get; set; }


        public virtual ICollection<Proveedor> Proveedores { get; set; }
        public virtual ICollection<Compania> Companias { get; set; }
    }
}
