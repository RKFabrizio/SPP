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

namespace SPP.Controllers
{
    [Route("api/[controller]/[action]")]
    public class CompaniasController : Controller
    {
        private SPPEU2GIGDEVSQLContext _context;

        public CompaniasController(SPPEU2GIGDEVSQLContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var companias = _context.Companias.Select(i => new {
                i.IdCompania,
                i.NombreCompania,
                i.IdPais
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "IdCompania" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(companias, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Compania();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Companias.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.IdCompania });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Companias.FirstOrDefaultAsync(item => item.IdCompania == key);
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
            var model = await _context.Companias.FirstOrDefaultAsync(item => item.IdCompania == key);

            _context.Companias.Remove(model);
            await _context.SaveChangesAsync();
        }


        [HttpGet]
        public async Task<IActionResult> PaisesLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Paises
                         orderby i.NombrePais
                         select new {
                             Value = i.IdPais,
                             Text = i.NombrePais
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        private void PopulateModel(Compania model, IDictionary values) {
            string ID_COMPANIA = nameof(Compania.IdCompania);
            string NOMBRE_COMPANIA = nameof(Compania.NombreCompania);
            string ID_PAIS = nameof(Compania.IdPais);

            if(values.Contains(ID_COMPANIA)) {
                model.IdCompania = Convert.ToInt32(values[ID_COMPANIA]);
            }

            if(values.Contains(NOMBRE_COMPANIA)) {
                model.NombreCompania = Convert.ToString(values[NOMBRE_COMPANIA]);
            }

            if(values.Contains(ID_PAIS)) {
                model.IdPais = Convert.ToInt32(values[ID_PAIS]);
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