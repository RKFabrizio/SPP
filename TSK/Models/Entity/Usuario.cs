using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SPP.Models.Entity;

namespace SPP.Models.Entity
{
    public partial class Usuario
    {
        public Usuario()
        {
            LoginSolicitante = new HashSet<Pago>();
            AprobadorAreas = new HashSet<Aprobador_Area>();
        }

        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Login { get; set; }
        public string Contrasena { get; set; }
        public string Correo { get; set; }
        public int MontoAprobacion { get; set; }
        public int IdPerfil { get; set; }
        public int? IdArea { get; set; }
        public bool Habilitado { get; set; }
        public string? Token { get; set; }
        public int IdCompania { get; set; }
        public int? IdPais { get; set; }


        public virtual Perfil PerfilDisNavigation { get; set; }
        public virtual Area AreaDisNavigation { get; set; }
        public virtual Compania CompaniaDisNavigation { get; set; }
        public virtual ICollection<Pago> LoginSolicitante { get; set; }
        public virtual ICollection<Aprobador_Area> AprobadorAreas { get; set; }
    }

}
