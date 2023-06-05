using System.ComponentModel.DataAnnotations;

namespace SPP.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El campo Perfil es obligatorio")]
        public int IdPerfil { get; set; }

        [Required(ErrorMessage = "El campo Area es obligatorio")]
        public int IdArea{ get; set; }

        [Required(ErrorMessage = "El campo Compania es obligatorio")]
        public int IdCompania { get; set; }

        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        public string Nombre { get; set; }
        public string Apellido { get; set; }

        public string Correo { get; set; }
        public string Login { get; set; }
        public int MontoAprobacion { get; set; }

        [Required(ErrorMessage = "El campo Contraseña es obligatorio")]
        public string Contrasena { get; set; }

        public string Token { get; set; }

        public int IdPais { get; set; }

        public string[] Perfiles { get; set; }
        public string[] Companias { get; set; }
        public string[] Areas { get; set; }

        public bool Habilitado { get; set; }
    }
}
