using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SPP.Models.Entity
{
    public partial class AprobadorArea
    {
        public int IdArea { get; set; }
        public int IdUsuario { get; set; }

        public virtual Area AreaDisNavigation { get; set; }
        public virtual Usuario UsuarioDisNavigation { get; set; }
    }
}
