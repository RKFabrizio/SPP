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
    public class ProveedorsController : Controller
    {
        private SPPEU2GIGDEVSQLContext _context;

        public ProveedorsController(SPPEU2GIGDEVSQLContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var proveedores = _context.Proveedores.Select(i => new {
                i.IdProveedor,
                i.NombreProveedor,
                i.IdTipoDoc,
                i.Numero_Doc,
                i.IdPais
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "IdProveedor" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(proveedores, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Proveedor();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Proveedores.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.IdProveedor });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Proveedores.FirstOrDefaultAsync(item => item.IdProveedor == key);
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
            var model = await _context.Proveedores.FirstOrDefaultAsync(item => item.IdProveedor == key);

            _context.Proveedores.Remove(model);
            await _context.SaveChangesAsync();
        }


        [HttpGet]
        public async Task<IActionResult> Tipo_DocumentosLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Tipo_Documentos
                         orderby i.NombreDocumento
                         select new {
                             Value = i.IdTipoDocumento,
                             Text = i.NombreDocumento
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
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

        private void PopulateModel(Proveedor model, IDictionary values) {
            string ID_PROVEEDOR = nameof(Proveedor.IdProveedor);
            string NOMBRE_PROVEEDOR = nameof(Proveedor.NombreProveedor);
            string ID_TIPO_DOC = nameof(Proveedor.IdTipoDoc);
            string NUMERO_DOC = nameof(Proveedor.Numero_Doc);
            string ID_PAIS = nameof(Proveedor.IdPais);

            if(values.Contains(ID_PROVEEDOR)) {
                model.IdProveedor = Convert.ToInt32(values[ID_PROVEEDOR]);
            }

            if(values.Contains(NOMBRE_PROVEEDOR)) {
                model.NombreProveedor = Convert.ToString(values[NOMBRE_PROVEEDOR]);
            }

            if(values.Contains(ID_TIPO_DOC)) {
                model.IdTipoDoc = Convert.ToInt32(values[ID_TIPO_DOC]);
            }

            if(values.Contains(NUMERO_DOC)) {
                model.Numero_Doc = Convert.ToString(values[NUMERO_DOC]);
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