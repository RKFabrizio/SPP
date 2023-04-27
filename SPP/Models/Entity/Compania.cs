using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SPP.Models.Entity
{
    public partial class Compania
    {
        public Compania()
        {
            Usuarios = new HashSet<Usuario>();
        }

        public int IdCompania { get; set; }
        public string NombreCompania { get; set; }
        public int IdPais { get; set; }
        public virtual Pais IdDisPais { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
