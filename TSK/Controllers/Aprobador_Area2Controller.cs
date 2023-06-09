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
    public class Aprobador_Area2Controller : Controller
    {
        private SPPEU2GIGDEVSQLContext _context;

        public Aprobador_Area2Controller(SPPEU2GIGDEVSQLContext context) {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions)
        {
            var aprobadorareas = _context.AprobadorAreas
                .GroupBy(a => a.IdUsuario)
                .Select(g => new {
                    IdAprobador = g.First().IdAprobador,
                    IdArea = g.First().IdArea,
                    IdUsuario = g.Key,
                    Correo = g.First().Correo
                });

            return Json(await DataSourceLoader.LoadAsync(aprobadorareas, loadOptions));
        }


        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Aprobador_Area();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.AprobadorAreas.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.IdAprobador });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.AprobadorAreas.FirstOrDefaultAsync(item => item.IdAprobador == key);
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
            var model = await _context.AprobadorAreas.FirstOrDefaultAsync(item => item.IdAprobador == key);

            _context.AprobadorAreas.Remove(model);
            await _context.SaveChangesAsync();
        }


        [HttpGet]
        public async Task<IActionResult> UsuariosLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Usuarios
                         orderby i.Nombre
                         select new {
                             Value = i.IdUsuario,
                             Text = i.Nombre
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

        private void PopulateModel(Aprobador_Area model, IDictionary values) {
            string ID_APROBADOR = nameof(Aprobador_Area.IdAprobador);
            string ID_AREA = nameof(Aprobador_Area.IdArea);
            string ID_USUARIO = nameof(Aprobador_Area.IdUsuario);
            string CORREO = nameof(Aprobador_Area.Correo);

            if(values.Contains(ID_APROBADOR)) {
                model.IdAprobador = Convert.ToInt32(values[ID_APROBADOR]);
            }

            if(values.Contains(ID_AREA)) {
                model.IdArea = Convert.ToInt32(values[ID_AREA]);
            }

            if(values.Contains(ID_USUARIO)) {
                model.IdUsuario = Convert.ToInt32(values[ID_USUARIO]);
            }

            if(values.Contains(CORREO)) {
                model.Correo = Convert.ToString(values[CORREO]);
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