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
    public class BancoesController : Controller
    {
        private SPPEU2GIGDEVSQLContext _context;

        public BancoesController(SPPEU2GIGDEVSQLContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var bancos = _context.Bancos
                .Where(p => p.IdBanco != 0)
                .Select(i => new {
                i.IdBanco,
                i.NombreBanco
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "IdBanco" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(bancos, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Banco();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Bancos.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.IdBanco });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Bancos.FirstOrDefaultAsync(item => item.IdBanco == key);
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
            var model = await _context.Bancos.FirstOrDefaultAsync(item => item.IdBanco == key);

            _context.Bancos.Remove(model);
            await _context.SaveChangesAsync();
        }


        private void PopulateModel(Banco model, IDictionary values) {
            string ID_BANCO = nameof(Banco.IdBanco);
            string NOMBRE_BANCO = nameof(Banco.NombreBanco);

            if(values.Contains(ID_BANCO)) {
                model.IdBanco = Convert.ToInt32(values[ID_BANCO]);
            }

            if(values.Contains(NOMBRE_BANCO)) {
                model.NombreBanco = Convert.ToString(values[NOMBRE_BANCO]);
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