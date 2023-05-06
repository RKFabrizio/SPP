using SPP.Models;
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using SPP.Data;

namespace SPP.Data
{
    public class UsuarioDatos
    {
        public List<Usuario> ListaUsuario()
        {
            var _usuario = new List<Usuario>();
            var cn = new Conexion();

            using (var conexion = new SqlConnection(cn.getCadenaSQL()))
            {
                conexion.Open();

                SqlCommand cmd = new SqlCommand("SP_Usuario_listar", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        _usuario.Add(new Usuario()
                        {
                            IdUsuario = Convert.ToInt32(dr["IDUSUARIO"]),
                            Nombre = dr["NOMBRE"].ToString(),
                            Apellido = dr["APELLIDO"].ToString(),
                            Login = dr["LOGIN"].ToString(),
                            Contrasena = dr["CONTRASENA"].ToString(),
                            Correo = dr["CORREO"].ToString(),
                            MontoAprobacion = Convert.ToInt32(dr["ID_POS"]),
                            IdPerfil = Convert.ToInt32(dr["IDPERFIL"]),
                            Perfiles = new string[] { dr["NOMBREPERFIL"].ToString() },
                            IdArea = Convert.ToInt32(dr["IDAREA"]),
                            Areas = new string[] { dr["NOMBREAREA"].ToString() },
                            Habilitado = Convert.ToBoolean(dr["HABILITADO"]),
                            IdCompania = Convert.ToInt32(dr["IDCOMPANIA"]),
                            Companias = new string[] { dr["NOMBRECOMPANIA"].ToString() }
                        });
                    }
                }
            }
            return _usuario;

        }

        public Usuario ValidarUsuario(string _correo, string _clave)
        {
            return ListaUsuario().Where(item => item.Login == _correo && item.Contrasena == _clave).FirstOrDefault();

        }
    }


}