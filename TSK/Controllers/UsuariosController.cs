using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using SPP.Models.Entity;

namespace TSK.Controllers
{
    [Route("api/[controller]/[action]")]
    public class UsuariosController : Controller
    {
        private SPPEU2GIGDEVSQLContext _context;

        public UsuariosController(SPPEU2GIGDEVSQLContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var usuarios = _context.Usuarios.Select(i => new {
                i.IdUsuario,
                i.Contrasena,
                i.Login,
                i.Nombre,
                i.Apellido,
                i.Correo,
                i.MontoAprobacion,
                i.IdPerfil,
                i.IdArea,
                i.IdCompania,
                i.Habilitado,
                i.Aprobador
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "IdUsuario" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(usuarios, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Usuario();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Usuarios.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.IdUsuario });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Usuarios.FirstOrDefaultAsync(item => item.IdUsuario == key);
            if(model == null)
                return StatusCode(409, "Object not found");

            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task Delete(int key) {
            var model = await _context.Usuarios.FirstOrDefaultAsync(item => item.IdUsuario == key);

            _context.Usuarios.Remove(model);
            await _context.SaveChangesAsync();
        }


        [HttpGet]
        public async Task<IActionResult> PerfilesLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Perfiles
                         orderby i.NombrePerfil
                         select new {
                             Value = i.IdPerfil,
                             Text = i.NombrePerfil
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> AreasLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Areas
                         orderby i.NombreArea
                         select new {
                             Value = i.IdArea,
                             Text = i.NombreArea
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> CompaniasLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Companias
                         orderby i.NombreCompania
                         select new {
                             Value = i.IdCompania,
                             Text = i.NombreCompania
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        private void PopulateModel(Usuario model, IDictionary values) {
            string ID_USUARIO = nameof(Usuario.IdUsuario);
            string CONTRASENA = nameof(Usuario.Contrasena);
            string LOGIN = nameof(Usuario.Login);
            string NOMBRE = nameof(Usuario.Nombre);
            string APELLIDO = nameof(Usuario.Apellido);
            string CORREO = nameof(Usuario.Correo);
            string MONTO_APROBACION = nameof(Usuario.MontoAprobacion);
            string ID_PERFIL = nameof(Usuario.IdPerfil);
            string ID_AREA = nameof(Usuario.IdArea);
            string ID_COMPANIA = nameof(Usuario.IdCompania);
            string HABILITADO = nameof(Usuario.Habilitado);
            string APROBADOR = nameof(Usuario.Aprobador);

            if(values.Contains(ID_USUARIO)) {
                model.IdUsuario = Convert.ToInt32(values[ID_USUARIO]);
            }

            if(values.Contains(CONTRASENA)) {
                model.Contrasena = Convert.ToString(values[CONTRASENA]);
            }

            if(values.Contains(LOGIN)) {
                model.Login = Convert.ToString(values[LOGIN]);
            }

            if(values.Contains(NOMBRE)) {
                model.Nombre = Convert.ToString(values[NOMBRE]);
            }

            if(values.Contains(APELLIDO)) {
                model.Apellido = Convert.ToString(values[APELLIDO]);
            }

            if(values.Contains(CORREO)) {
                model.Correo = Convert.ToString(values[CORREO]);
            }

            if(values.Contains(MONTO_APROBACION)) {
                model.MontoAprobacion = Convert.ToInt32(values[MONTO_APROBACION]);
            }

            if(values.Contains(ID_PERFIL)) {
                model.IdPerfil = Convert.ToInt32(values[ID_PERFIL]);
            }

            if(values.Contains(ID_AREA)) {
                model.IdArea = Convert.ToInt32(values[ID_AREA]);
            }

            if(values.Contains(ID_COMPANIA)) {
                model.IdCompania = Convert.ToInt32(values[ID_COMPANIA]);
            }

            if(values.Contains(HABILITADO)) {
                model.Habilitado = Convert.ToBoolean(values[HABILITADO]);
            }

            if(values.Contains(APROBADOR)) {
                model.Aprobador = Convert.ToBoolean(values[APROBADOR]);
            }
        }

        private string GetFullErrorMessage(ModelStateDictionary modelState) {
            var messages = new List<string>();

            foreach(var entry in modelState) {
                foreach(var error in entry.Value.Errors)
                    messages.Add(error.ErrorMessage);
            }

            return String.Join(" ", messages);
        }
    }
}