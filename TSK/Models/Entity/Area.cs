using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SPP.Models.Entity
{
    public partial class Area
    {
        public Area()
        {
            Usuarios = new HashSet<Usuario>();
        }

        public int IdArea { get; set; }
        public string NombreArea { get; set; }

        public virtual ICollection<Usuario> Usuarios { get; set; }

    }
}
