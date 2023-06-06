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
using NuGet.Protocol;
using System.Net.Mail;
using System.Web;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using System.Net;
using Microsoft.AspNetCore.Hosting;



namespace TSK.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PagoesController : Controller
    {
        private SPPEU2GIGDEVSQLContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public PagoesController(IWebHostEnvironment webHostEnvironment, SPPEU2GIGDEVSQLContext context)
        {
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }



        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {

            var pagos = _context.Pagos.Select(i => new {
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

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "IdPago" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(pagos, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(Pago model, List<IFormFile> ReferenciaOC, List<IFormFile> Proformacotizacion, List<IFormFile> Factura)
        {

            // Guardar archivos PDF en una carpeta
            string folderPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Media");


            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }


            foreach (var file in ReferenciaOC)
            {
                string fileName = file.FileName;
                string filePath = Path.Combine(folderPath, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                Console.WriteLine($"Archivo ReferenciaOC guardado: {filePath}");
            }

            foreach (var file in Proformacotizacion)
            {
                string fileName = file.FileName;
                string filePath = Path.Combine(folderPath, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                Console.WriteLine($"Archivo Proformacotizacion guardado: {filePath}");
            }

            foreach (var file in Factura)
            {
                string fileName = file.FileName;
                string filePath = Path.Combine(folderPath, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                Console.WriteLine($"Archivo Factura guardado: {filePath}");
            }

            if (model.LoginAprobador == 0)
            {
                model.LoginAprobador = 12;  // Valor por defecto
            }

            //// Realiza las operaciones necesarias con el objeto "model"
            //Console.WriteLine(model.ToJson());

            //// Valida el modelo
            //if (!TryValidateModel(model))
            //    return BadRequest(GetFullErrorMessage(ModelState));

            //// Asigna la fecha y hora actuales a FechaSolicitud
            //model.FechaSolicitud = DateTime.Now;

            //string usuarioInfoJson = HttpContext.Request.Cookies["UsuarioInfo"];
            //if (!string.IsNullOrEmpty(usuarioInfoJson))
            //{
            //    Usuario usuario = JsonConvert.DeserializeObject<Usuario>(usuarioInfoJson);
            //    int? AreaUsuario = usuario.IdArea;

            //    // Crea una consulta de uni�n para combinar la informaci�n de las tablas AprobadorArea y Usuarios
            //    var aprobadoresArea = _context.AprobadorAreas
            //        .Join(_context.Usuarios,
            //            aprobador => aprobador.IdUsuario,
            //            usuario => usuario.IdUsuario,
            //            (aprobador, usuario) => new { Aprobador = aprobador, Usuario = usuario })
            //        .Where(aprobadorUsuario => aprobadorUsuario.Aprobador.IdArea == AreaUsuario)
            //        .ToList();

            //    // Filtra la lista de aprobadores para encontrar el que tiene la mayor capacidad de aprobaci�n
            //    // que a�n sea igual o menor al importe. Si no se encuentra ninguno, usa el IdAprobador = 49.
            //    var aprobador = aprobadoresArea
            //    .Where(aprobadorUsuario => aprobadorUsuario.Usuario.MontoAprobacion >= model.Importe)
            //    .OrderBy(aprobadorUsuario => aprobadorUsuario.Usuario.MontoAprobacion)
            //    .FirstOrDefault();


            //    if (aprobador != null)
            //    {
            //        model.LoginAprobador = aprobador.Aprobador.IdUsuario;
            //    }
            //    else
            //    {
            //        model.LoginAprobador = 49; // Asigna el valor 49 si no se encuentra un aprobador adecuado.
            //    }

            //}

            // Agrega el modelo a la base de datos
            var result = _context.Pagos.Add(model);

            //await _context.SaveChangesAsync();
            //string correo_emisor = "leedryk@gmail.com";
            //string clave_emisor = "xxrlviitjlpqytrj";

            //MailAddress receptor = new("fabriziosebastianbusiness@gmail.com");
            //MailAddress emisor = new("leedryk@gmail.com");

            //MailMessage email = new MailMessage(emisor, receptor);
            //email.Subject = "Pruebas para SPP";

            //string body = @"
            //<div style='background-color: #F1F0E9; padding: 20px; width: 715px; text-align: center;'>
            //<h2 style='font-weight: bold; font-size: 22px; color: #000000;'>SOLICITUD DE PAGO DE PROVEEDORES</h2>
            //<div style='text-align: left; width: 666px; background: #F1F0E9; border: 2px solid #DDDAD2; padding: 10px;'>
            //    <label style='font-size: 15px; color: #000000;'>N�mero de Solicitud:</label>
            //    <br/>
            //    <div style='border: 1px solid #A79A66; width: 301px; height: 20px;'>"
            //        + model.IdPago +
            //    @"</div>
            //    </div>
            //</div>";

            //email.Body = body;
            //email.IsBodyHtml = true;  // Indicate that the email body is HTML

            //SmtpClient smtp = new();
            //smtp.Host = "smtp.gmail.com";
            //smtp.Port = 587;
            //smtp.Credentials = new NetworkCredential(correo_emisor, clave_emisor);
            //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            //smtp.EnableSsl = true;

            //smtp.Send(email);

            //return Json(new { IdPago = result.Entity.IdPago });
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
        public async Task<IActionResult> TipoAdelantos1Lookup(DataSourceLoadOptions loadOptions)
        {
            var lookup = from i in _context.TipoAdelantos
                         where i.IdTipoAdelanto == 1
                         orderby i.TipoAdelanto
                         select new
                         {
                             Value = i.IdTipoAdelanto,
                             Text = i.TipoAdelanto
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }


        [HttpGet]
        public async Task<IActionResult> TipoAdelantos2Lookup(DataSourceLoadOptions loadOptions)
        {
            var lookup = from i in _context.TipoAdelantos
                         where i.IdTipoAdelanto == 2
                         orderby i.TipoAdelanto
                         select new
                         {
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
        public async Task<IActionResult> UsuariosLookup(DataSourceLoadOptions loadOptions)
        {
            // Obtener la informaci�n del usuario actual
            string usuarioInfoJson = HttpContext.Request.Cookies["UsuarioInfo"];
            if (!string.IsNullOrEmpty(usuarioInfoJson))
            {
                Usuario usuarioActual = JsonConvert.DeserializeObject<Usuario>(usuarioInfoJson);
                int idUsuario = usuarioActual.IdUsuario;  // Aqu� obtenemos el IdUsuario

                var lookup = from i in _context.Usuarios
                             where i.IdUsuario == idUsuario  // Filtrar por IdUsuario
                             orderby i.Nombre
                             select new
                             {
                                 Value = i.IdUsuario,
                                 Text = i.Nombre
                             };
                return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
            }
            else
            {
                // Si no hay informaci�n del usuario en la cookie, puedes manejarlo de la manera que prefieras.
                // Por ejemplo, podr�as redirigir al usuario a la p�gina de inicio de sesi�n.
                return RedirectToAction("Login", "Acceso");
            }
        }

        [HttpGet]
        public async Task<IActionResult> AprobadorAreasLookup([FromQuery] DataSourceLoadOptions loadOptions, [FromQuery] int importe)
        {
            Console.WriteLine("HOLAAA" + importe);
            System.Console.WriteLine(importe);
            var lookup = from i in _context.AprobadorAreas
                         join u in _context.Usuarios on i.IdUsuario equals u.IdUsuario
                         group new { i, u } by i.IdUsuario into g
                         select new
                         {
                             Value = g.First().i.IdAprobador,
                             Text = g.First().u.Nombre + " " + g.First().u.Apellido
                         };

            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }




        [HttpGet]
        public async Task<IActionResult> TipoPagosLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.TipoPagos
                         where i.IdTipoPago != 0
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
                         where i.IdBanco != 0
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
                         where i.IdTipoCuenta != 0
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
                model.IdTipoPago = Convert.ToInt32(values[ID_TIPO_PAGO]);
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
                model.IdBanco = Convert.ToInt32(values[ID_BANCO]);
            }

            if(values.Contains(ID_TIPO_CUENTA)) {
                model.IdTipoCuenta = Convert.ToInt32(values[ID_TIPO_CUENTA]);
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