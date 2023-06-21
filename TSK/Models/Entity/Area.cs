using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SPP.Models.Entity;

namespace SPP.Models.Entity
{
    public partial class Area
    {
        public Area()
        {
            Usuarios = new HashSet<Usuario>();
            AprobadorAreas = new HashSet<Aprobador_Area>();
        }

        public int IdArea { get; set; }
        public string? NombreArea { get; set; }

        public virtual ICollection<Usuario> Usuarios { get; set; }
        public virtual ICollection<Aprobador_Area> AprobadorAreas { get; set; }
    }
}
