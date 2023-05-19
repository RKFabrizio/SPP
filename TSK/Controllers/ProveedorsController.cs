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
                i.CuentaBancaria,
                i.CCI,
                i.BeneficiarioNombre,
                i.BeneficiarioDni,
                i.IdBanco,
                i.IdTipoCuenta,
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
        public async Task<IActionResult> BancosLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Bancos
                         orderby i.NombreBanco
                         select new {
                             Value = i.IdBanco,
                             Text = i.NombreBanco
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> TipoCuentasLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.TipoCuentas
                         orderby i.TipoCuenta
                         select new {
                             Value = i.IdTipoCuenta,
                             Text = i.TipoCuenta
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
            string CUENTA_BANCARIA = nameof(Proveedor.CuentaBancaria);
            string CCI = nameof(Proveedor.CCI);
            string BENEFICIARIO_NOMBRE = nameof(Proveedor.BeneficiarioNombre);
            string BENEFICIARIO_DNI = nameof(Proveedor.BeneficiarioDni);
            string ID_BANCO = nameof(Proveedor.IdBanco);
            string ID_TIPO_CUENTA = nameof(Proveedor.IdTipoCuenta);
            string ID_PAIS = nameof(Proveedor.IdPais);

            if(values.Contains(ID_PROVEEDOR)) {
                model.IdProveedor = Convert.ToInt32(values[ID_PROVEEDOR]);
            }

            if(values.Contains(NOMBRE_PROVEEDOR)) {
                model.NombreProveedor = Convert.ToString(values[NOMBRE_PROVEEDOR]);
            }

            if(values.Contains(CUENTA_BANCARIA)) {
                model.CuentaBancaria = Convert.ToString(values[CUENTA_BANCARIA]);
            }

            if(values.Contains(CCI)) {
                model.CCI = Convert.ToString(values[CCI]);
            }

            if(values.Contains(BENEFICIARIO_NOMBRE)) {
                model.BeneficiarioNombre = Convert.ToString(values[BENEFICIARIO_NOMBRE]);
            }

            if(values.Contains(BENEFICIARIO_DNI)) {
                model.BeneficiarioDni = Convert.ToString(values[BENEFICIARIO_DNI]);
            }

            if(values.Contains(ID_BANCO)) {
                model.IdBanco = Convert.ToInt32(values[ID_BANCO]);
            }

            if(values.Contains(ID_TIPO_CUENTA)) {
                model.IdTipoCuenta = Convert.ToInt32(values[ID_TIPO_CUENTA]);
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