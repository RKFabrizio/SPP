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
    public class PagoAdmin2Controller : Controller
    {
        private SPPEU2GIGDEVSQLContext _context;

        public PagoAdmin2Controller(SPPEU2GIGDEVSQLContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions)
        {
            // Obtener la informaci�n del usuario actual
            string usuarioInfoJson = HttpContext.Request.Cookies["UsuarioInfo"];
            if (!string.IsNullOrEmpty(usuarioInfoJson))
            {
                Usuario usuario = JsonConvert.DeserializeObject<Usuario>(usuarioInfoJson);
                int idUsuario = usuario.IdUsuario;  // Aqu� obtenemos el IdUsuario
                int idCompania = usuario.IdCompania;  // Aqu� obtenemos el IdPais

                var pagos = _context.Pagos
                    .Join(_context.Usuarios,  // Tabla a la que nos unimos
                          p => p.LoginSolicitante,  // Propiedad en la tabla principal (Pagos)
                          u => u.IdUsuario,  // Propiedad en la tabla de uni�n (Usuarios)
                          (p, u) => new { Pago = p, Usuario = u })  // Resultado de la uni�n
                    .Where(pu => pu.Usuario.IdCompania == idCompania)  // Filtrar por LoginSolicitante y IdPais
                    .Select(i => new {
                        i.Pago.IdPago,
                        i.Pago.IdTipoAdelanto,
                        i.Pago.IdProveedor,
                        i.Pago.FechaSolicitud,
                        i.Pago.IdTipoMoneda,
                        i.Pago.Importe,
                        i.Pago.Concepto,
                        i.Pago.LoginSolicitante,
                        i.Pago.LoginAprobador,
                        i.Pago.ReferenciaOC,
                        i.Pago.ProformaCotizacion,
                        i.Pago.Factura,
                        i.Pago.IdTipoPago,
                        i.Pago.Observaciones,
                        i.Pago.FechaAprobacion,
                        i.Pago.IdEstado,
                        i.Pago.InformacionContable,
                        i.Pago.CuentaBancaria,
                        i.Pago.BeneficiarioNombre,
                        i.Pago.BeneficiarioDni,
                        i.Pago.IdBanco,
                        i.Pago.IdTipoCuenta,
                        i.Pago.Compania
                    })
                    .OrderByDescending(i => i.FechaSolicitud);  // Ordenar por FechaSolicitud en orden descendente

                return Json(await DataSourceLoader.LoadAsync(pagos, loadOptions));
            }
            else
            {
                // Si no hay informaci�n del usuario en la cookie, puedes manejarlo de la manera que prefieras.
                // Por ejemplo, podr�as redirigir al usuario a la p�gina de inicio de sesi�n.
                return RedirectToAction("Login", "Acceso");
            }
        }






        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Pago();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Pagos.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.IdPago });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Pagos.FirstOrDefaultAsync(item => item.IdPago == key);
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
            var model = await _context.Pagos.FirstOrDefaultAsync(item => item.IdPago == key);

            _context.Pagos.Remove(model);
            await _context.SaveChangesAsync();
        }


        [HttpGet]
        public async Task<IActionResult> TipoAdelantosLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.TipoAdelantos
                         orderby i.TipoAdelanto
                         select new {
                             Value = i.IdTipoAdelanto,
                             Text = i.TipoAdelanto
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> ProveedoresLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Proveedores
                         orderby i.NombreProveedor
                         select new {
                             Value = i.IdProveedor,
                             Text = i.NombreProveedor
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> TipoMonedasLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.TipoMonedas
                         orderby i.TipoMoneda
                         select new {
                             Value = i.IdTipoMoneda,
                             Text = i.TipoMoneda
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
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
        public async Task<IActionResult> AprobadorAreasLookup(DataSourceLoadOptions loadOptions)
        {
            var lookup = from i in _context.AprobadorAreas
                         join u in _context.Usuarios on i.IdUsuario equals u.IdUsuario
                         orderby i.IdUsuario
                         select new
                         {
                             Value = i.IdUsuario,
                             Text = u.Nombre + " " + u.Apellido // Aqu� estamos seleccionando el nombre del usuario
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }


        [HttpGet]
        public async Task<IActionResult> TipoPagosLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.TipoPagos
                         orderby i.TipoPago
                         select new {
                             Value = i.IdTipoPago,
                             Text = i.TipoPago
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> EstadosLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Estados
                         orderby i.NombreEstado
                         select new {
                             Value = i.IdEstado,
                             Text = i.NombreEstado
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
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

        private void PopulateModel(Pago model, IDictionary values) {
            string ID_PAGO = nameof(Pago.IdPago);
            string ID_TIPO_ADELANTO = nameof(Pago.IdTipoAdelanto);
            string ID_PROVEEDOR = nameof(Pago.IdProveedor);
            string FECHA_SOLICITUD = nameof(Pago.FechaSolicitud);
            string ID_TIPO_MONEDA = nameof(Pago.IdTipoMoneda);
            string IMPORTE = nameof(Pago.Importe);
            string CONCEPTO = nameof(Pago.Concepto);
            string LOGIN_SOLICITANTE = nameof(Pago.LoginSolicitante);
            string LOGIN_APROBADOR = nameof(Pago.LoginAprobador);
            string REFERENCIA_OC = nameof(Pago.ReferenciaOC);
            string PROFORMA_COTIZACION = nameof(Pago.ProformaCotizacion);
            string FACTURA = nameof(Pago.Factura);
            string ID_TIPO_PAGO = nameof(Pago.IdTipoPago);
            string OBSERVACIONES = nameof(Pago.Observaciones);
            string FECHA_APROBACION = nameof(Pago.FechaAprobacion);
            string ID_ESTADO = nameof(Pago.IdEstado);
            string INFORMACION_CONTABLE = nameof(Pago.InformacionContable);
            string CUENTA_BANCARIA = nameof(Pago.CuentaBancaria);
            string BENEFICIARIO_NOMBRE = nameof(Pago.BeneficiarioNombre);
            string BENEFICIARIO_DNI = nameof(Pago.BeneficiarioDni);
            string ID_BANCO = nameof(Pago.IdBanco);
            string ID_TIPO_CUENTA = nameof(Pago.IdTipoCuenta);

            if(values.Contains(ID_PAGO)) {
                model.IdPago = Convert.ToInt32(values[ID_PAGO]);
            }

            if(values.Contains(ID_TIPO_ADELANTO)) {
                model.IdTipoAdelanto = Convert.ToInt32(values[ID_TIPO_ADELANTO]);
            }

            if(values.Contains(ID_PROVEEDOR)) {
                model.IdProveedor = Convert.ToInt32(values[ID_PROVEEDOR]);
            }

            if(values.Contains(FECHA_SOLICITUD)) {
                model.FechaSolicitud = Convert.ToDateTime(values[FECHA_SOLICITUD]);
            }

            if(values.Contains(ID_TIPO_MONEDA)) {
                model.IdTipoMoneda = Convert.ToInt32(values[ID_TIPO_MONEDA]);
            }

            if(values.Contains(IMPORTE)) {
                model.Importe = Convert.ToSingle(values[IMPORTE], CultureInfo.InvariantCulture);
            }

            if(values.Contains(CONCEPTO)) {
                model.Concepto = Convert.ToString(values[CONCEPTO]);
            }

            if(values.Contains(LOGIN_SOLICITANTE)) {
                model.LoginSolicitante = Convert.ToInt32(values[LOGIN_SOLICITANTE]);
            }

            if(values.Contains(LOGIN_APROBADOR)) {
                model.LoginAprobador = Convert.ToInt32(values[LOGIN_APROBADOR]);
            }

            if(values.Contains(REFERENCIA_OC)) {
                model.ReferenciaOC = Convert.ToString(values[REFERENCIA_OC]);
            }

            if(values.Contains(PROFORMA_COTIZACION)) {
                model.ProformaCotizacion = Convert.ToString(values[PROFORMA_COTIZACION]);
            }

            if(values.Contains(FACTURA)) {
                model.Factura = Convert.ToString(values[FACTURA]);
            }

            if(values.Contains(ID_TIPO_PAGO)) {
                model.IdTipoPago = values[ID_TIPO_PAGO] != null ? Convert.ToInt32(values[ID_TIPO_PAGO]) : (int?)null;
            }

            if(values.Contains(OBSERVACIONES)) {
                model.Observaciones = Convert.ToString(values[OBSERVACIONES]);
            }

            if(values.Contains(FECHA_APROBACION)) {
                model.FechaAprobacion = values[FECHA_APROBACION] != null ? Convert.ToDateTime(values[FECHA_APROBACION]) : (DateTime?)null;
            }

            if(values.Contains(ID_ESTADO)) {
                model.IdEstado = Convert.ToInt32(values[ID_ESTADO]);
            }

            if(values.Contains(INFORMACION_CONTABLE)) {
                model.InformacionContable = Convert.ToString(values[INFORMACION_CONTABLE]);
            }

            if(values.Contains(CUENTA_BANCARIA)) {
                model.CuentaBancaria = Convert.ToString(values[CUENTA_BANCARIA]);
            }

            if(values.Contains(BENEFICIARIO_NOMBRE)) {
                model.BeneficiarioNombre = Convert.ToString(values[BENEFICIARIO_NOMBRE]);
            }

            if(values.Contains(BENEFICIARIO_DNI)) {
                model.BeneficiarioDni = Convert.ToString(values[BENEFICIARIO_DNI]);
            }

            if(values.Contains(ID_BANCO)) {
                model.IdBanco = values[ID_BANCO] != null ? Convert.ToInt32(values[ID_BANCO]) : (int?)null;
            }

            if(values.Contains(ID_TIPO_CUENTA)) {
                model.IdTipoCuenta = values[ID_TIPO_CUENTA] != null ? Convert.ToInt32(values[ID_TIPO_CUENTA]) : (int?)null;
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