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
using System.Net.Mail;
using System.Net;

namespace TSK.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PagoMiAprobadorController : Controller
    {
        private SPPEU2GIGDEVSQLContext _context;

        public PagoMiAprobadorController(SPPEU2GIGDEVSQLContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions)
        {
            // Obtener la información del usuario actual
            string usuarioInfoJson = HttpContext.Request.Cookies["UsuarioInfo"];
            if (!string.IsNullOrEmpty(usuarioInfoJson))
            {
                Usuario usuario = JsonConvert.DeserializeObject<Usuario>(usuarioInfoJson);
                int idUsuario = usuario.IdUsuario;  // Aquí obtenemos el IdUsuario

                var pagos = _context.Pagos
                    .Where(p => p.LoginAprobador == idUsuario && (p.IdEstado == 0))  // Filtrar por LoginSolicitante
                    .Select(i => new {
                        i.IdPago,
                        i.IdTipoAdelanto,
                        i.IdProveedor,
                        i.FechaSolicitud,
                        i.IdTipoMoneda,
                        i.Importe,
                        i.Concepto,
                        i.LoginSolicitante,
                        i.LoginAprobador,
                        i.ReferenciaOC,
                        i.ProformaCotizacion,
                        i.Factura,
                        i.IdTipoPago,
                        i.Observaciones,
                        i.FechaAprobacion,
                        i.IdEstado,
                        i.InformacionContable,
                        i.CuentaBancaria,
                        i.BeneficiarioNombre,
                        i.BeneficiarioDni,
                        i.IdBanco,
                        i.IdTipoCuenta
                    });

                return Json(await DataSourceLoader.LoadAsync(pagos, loadOptions));
            }
            else
            {
                // Si no hay información del usuario en la cookie, puedes manejarlo de la manera que prefieras.
                // Por ejemplo, podrías redirigir al usuario a la página de inicio de sesión.
                return RedirectToAction("Login", "Acceso");
            }
        }


        [HttpPost]
        public async Task<IActionResult> Post(string values)
        {
            var model = new Pago();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if (!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Pagos.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.IdPago });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values)
        {             

            var model = await _context.Pagos.FirstOrDefaultAsync(item => item.IdPago == key);
                if (model == null)
                    return StatusCode(409, "Object not found");

                var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
                PopulateModel(model, valuesDict);

                if (!TryValidateModel(model))
                    return BadRequest(GetFullErrorMessage(ModelState));

                // Solo actualiza FechaAprobacion si IdEstado es 1
                if (model.IdEstado == 1)
                {
                    model.FechaAprobacion = DateTime.Now;
                }

            // Obtener LoginSolicitante del IdPago que se está editando
            int idSolicitante = model.LoginSolicitante;

            // Usar _context.Usuarios para buscar el correo mediante idUsuario
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == idSolicitante);
            if (usuario == null)
                return StatusCode(409, "Usuario not found");

            string correoSolicitante = usuario.Correo;
            var estado = await _context.Estados.FirstOrDefaultAsync(p => p.IdEstado == model.IdEstado);


            await _context.SaveChangesAsync();

            string correo_emisor = "svc-vd-pino@barrick.com";
            string clave_emisor = "maVafRevUp23";

            //string correo_emisor = "leedryk@gmail.com";
            //string clave_emisor = "xxrlviitjlpqytrj";

            MailAddress receptor = new MailAddress(correoSolicitante);
            MailAddress emisor = new MailAddress(correo_emisor);

            MailMessage email = new MailMessage(emisor, receptor);

            email.Subject = "Sistema Pago de Proveedores";

            string body = @"Tu solicitud Nro " + model.IdPago + @" fue: " + estado.NombreEstado + @" ";

            email.Body = body;
            email.IsBodyHtml = true;  // Indicate that the email body is HTML


            //SmtpClient smtp = new();
            //smtp.Host = "smtp.gmail.com";
            //smtp.Port = 587;
            //smtp.Credentials = new NetworkCredential(correo_emisor, clave_emisor);
            //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            //smtp.EnableSsl = true;

            var client = new SmtpClient("CHISANEMP1");
            client.Credentials = new System.Net.NetworkCredential("svc-vd-pino@barrick.com", "");
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Send(email);


            //smtp.Send(email);

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
                             Text = u.Nombre + " " + u.Apellido // Aquí estamos seleccionando el nombre del usuario
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
                         where i.IdEstado != 2
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