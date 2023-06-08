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
using DevExpress.Data.Mask.Internal;

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
            // Obtiene el NombreProveedor usando el IdProveedor
            var proveedor = await _context.Proveedores.FirstOrDefaultAsync(p => p.IdProveedor == model.IdProveedor);
            // Obtiene el Usuario - Nombre
            string usuarioInfoJson = HttpContext.Request.Cookies["UsuarioInfo"];
            string solicitante = string.Empty; // Declaración de la variable en un nivel superior.

            if (!string.IsNullOrEmpty(usuarioInfoJson))
            {
                Usuario usuario = JsonConvert.DeserializeObject<Usuario>(usuarioInfoJson);
                solicitante = usuario.Nombre + " " + usuario.Apellido;
            }
            //Obtener variables de datos
            var adelanto = await _context.TipoAdelantos.FirstOrDefaultAsync(p => p.IdTipoAdelanto == model.IdTipoAdelanto);
            var moneda = await _context.TipoMonedas.FirstOrDefaultAsync(p => p.IdTipoMoneda == model.IdTipoMoneda);


            string folderPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Mediaa");


            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            string lastReferenciaOCFilePath = null;
            foreach (var file in ReferenciaOC)
            {
                string fileName = file.FileName;
                string filePath = Path.Combine(folderPath, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                Console.WriteLine($"Archivo ReferenciaOC guardado: {filePath}");
                model.ReferenciaOC = fileName;
                lastReferenciaOCFilePath = filePath;
            }
            string lastProformacotizacionFilePath = null;
            foreach (var file in Proformacotizacion)
            {
                string fileName = file.FileName;
                string filePath = Path.Combine(folderPath, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                Console.WriteLine($"Archivo Proformacotizacion guardado: {filePath}");
                model.ProformaCotizacion = fileName;
                lastProformacotizacionFilePath = filePath;
            }

            string lastFacturaFilePath = null;
            foreach (var file in Factura)
            {
                string fileName = file.FileName;
                string filePath = Path.Combine(folderPath, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                Console.WriteLine($"Archivo Factura guardado: {filePath}");
                model.Factura = fileName;
                lastFacturaFilePath = filePath;
            }


            if (model.LoginAprobador == 0)
            {
                model.LoginAprobador = 12;  // Valor por defecto
            }

            // Realiza las operaciones necesarias con el objeto "model"
            Console.WriteLine(model.ToJson());

            // Valida el modelo
            if (!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            // Asigna la fecha y hora actuales a FechaSolicitud
            model.FechaSolicitud = DateTime.Now;

            if (!string.IsNullOrEmpty(usuarioInfoJson))
            {
                Usuario usuario = JsonConvert.DeserializeObject<Usuario>(usuarioInfoJson);
                int? AreaUsuario = usuario.IdArea;

                // Crea una consulta de unión para combinar la información de las tablas AprobadorArea y Usuarios
                var aprobadoresArea = _context.AprobadorAreas
                    .Join(_context.Usuarios,
                        aprobador => aprobador.IdUsuario,
                        usuario => usuario.IdUsuario,
                        (aprobador, usuario) => new { Aprobador = aprobador, Usuario = usuario })
                    .Where(aprobadorUsuario => aprobadorUsuario.Aprobador.IdArea == AreaUsuario)
                    .ToList();

                // Filtra la lista de aprobadores para encontrar el que tiene la mayor capacidad de aprobación
                // que aún sea igual o menor al importe. Si no se encuentra ninguno, usa el IdAprobador = 49.
                var aprobador = aprobadoresArea
                .Where(aprobadorUsuario => aprobadorUsuario.Usuario.MontoAprobacion >= model.Importe)
                .OrderBy(aprobadorUsuario => aprobadorUsuario.Usuario.MontoAprobacion)
                .FirstOrDefault();


                if (aprobador != null)
                {
                    model.LoginAprobador = aprobador.Aprobador.IdUsuario;
                }
                else
                {
                    model.LoginAprobador = 49; // Asigna el valor 49 si no se encuentra un aprobador adecuado.
                }

            }

            // Obtiene el ID del usuario aprobador
            var aprobadorId = model.LoginAprobador;

            // Busca al usuario aprobador en la base de datos
            var aprobador1 = await _context.Usuarios.FindAsync(aprobadorId);

            // Si el usuario aprobador no se encuentra en la base de datos, devuelve un error
            if (aprobador1 == null)
            {
                return NotFound($"No se pudo encontrar un usuario con el ID {aprobadorId}");
            }

            // Obtiene el correo electrónico del usuario aprobador
            var correoAprobador = aprobador1.Correo;

            // Agrega el modelo a la base de datos
            var result = _context.Pagos.Add(model);



            await _context.SaveChangesAsync();
            string correo_emisor = "leedryk@gmail.com";
            string clave_emisor = "xxrlviitjlpqytrj";

            MailAddress receptor = new(correoAprobador);
            MailAddress emisor = new("leedryk@gmail.com");

            MailMessage email = new MailMessage(emisor, receptor);
            if (lastFacturaFilePath != null)
            {
                Attachment attachment = new Attachment(lastFacturaFilePath);
                email.Attachments.Add(attachment);
            }
            if (lastReferenciaOCFilePath != null)
            {
                Attachment attachment = new Attachment(lastReferenciaOCFilePath);
                email.Attachments.Add(attachment);
            }
            if (lastProformacotizacionFilePath != null)
            {
                Attachment attachment = new Attachment(lastProformacotizacionFilePath);
                email.Attachments.Add(attachment);
            }

            email.Subject = "Pruebas para SPP";

            string body = @"
            <div style='background-color: #f1f0e9; padding: 20px; width: 715px; text-align: center;'>
                <h2 style='font-weight: bold; font-size: 23px; color: #000000;'>Solicitud de pago de proveedores</h2>
                <div style='text-align: left; width: 666px; background: #f1f0e9; border: 2px solid #dddad2; padding: 10px;'>
                    <table>
                        <tr>
                            <td>
                                <label style='font-size: 15px; color: #000000;'>Número de solicitud:</label>
                                <br/>
                                <div style='border: 1px solid #a79a66; width: 301px; height: 20px;'>"
                                     + model.IdPago +
                                 @"</div>
                            </td>
                            <td>
                                <label style='font-size: 15px; color: #000000;'>Tipo de Solicitud:</label>
                                <br/>
                                <div style='border: 1px solid #a79a66; width: 301px; height: 20px;'>"
                                     + adelanto.TipoAdelanto +
                                 @"</div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan='2'>
                                <label style='font-size: 15px; color: #000000;'>Nombre de Proveedor y/o Beneficiario:</label>
                                <br/>
                                <div style='border: 1px solid #a79a66; width: 500px; height: 20px;'>"
                                     + proveedor.NombreProveedor +
                                 @"</div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label style='font-size: 15px; color: #000000;'>Fecha de Solicitud:</label>
                                <br/>
                                <div style='border: 1px solid #a79a66; width: 301px; height: 20px;'>"
                                     + model.FechaSolicitud +
                                 @"</div>
                            </td>
                                <td>
                                <div style='display: inline-block; vertical-align: top;'>
                                    <label style='font-size: 15px; color: #000000;'>Importe:</label>
                                    <br/>
                                    <div style='border: 1px solid #a79a66; width: 105px; height: 20px;'>"
                                         + model.Importe + ".00" +
                                     @"</div>
                                </div>
                                <div style='display: inline-block; vertical-align: top; margin-left: 20px;'>
                                    <label style='font-size: 15px; color: #000000;'>Moneda:</label>
                                    <br/>
                                    <div style='border: 1px solid #a79a66; width: 105px; height: 20px;'>"
                                         + moneda.TipoMoneda +
                                     @"</div>
                                </div>
                            </td>

                        </tr>
                        <tr>
                            <td colspan='2'>
                                <label style='font-size: 15px; color: #000000;'>Concepto:</label>
                                <br/>
                                <div style='border: 1px solid #a79a66; width: 500px; height: 20px;'>"
                                     + model.Concepto +
                                 @"</div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan='2'>
                                <label style='font-size: 15px; color: #000000;'>Observaciones:</label>
                                <br/>
                                <div style='border: 1px solid #a79a66; width: 500px; height: 20px;'>"
                                     + model.Observaciones +
                                 @"</div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan='2'>
                                <label style='font-size: 15px; color: #000000;'>Solicitante:</label>
                                <br/>
                                <div style='border: 1px solid #a79a66; width: 301px; height: 20px;'>"
                                    + solicitante +
                                @"</div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>";


            email.Body = body;
            email.IsBodyHtml = true;  // Indicate that the email body is HTML

            SmtpClient smtp = new();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.Credentials = new NetworkCredential(correo_emisor, clave_emisor);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = true;

            smtp.Send(email);

            return Json(new { IdPago = result.Entity.IdPago });
        }

        [HttpPost]
        public async Task<IActionResult> Post2(Pago model, List<IFormFile> ReferenciaOC, List<IFormFile> Proformacotizacion, List<IFormFile> Factura)
        {
            // Obtiene el NombreProveedor usando el IdProveedor
            var proveedor = await _context.Proveedores.FirstOrDefaultAsync(p => p.IdProveedor == model.IdProveedor);
            if (proveedor != null)
            {
                model.BeneficiarioNombre = proveedor.NombreProveedor;
            }
            else
            {
                return BadRequest("No se pudo encontrar un proveedor con el ID especificado");
            }

            //Obtiene el Usuario - Nombre
             string usuarioInfoJson = HttpContext.Request.Cookies["UsuarioInfo"];
            if (!string.IsNullOrEmpty(usuarioInfoJson))
            {
                Usuario usuario = JsonConvert.DeserializeObject<Usuario>(usuarioInfoJson);
                string solicitante = usuario.Nombre + " " + usuario.Apellido;
            }
            //Obtener variables de datos
            var adelanto = await _context.TipoAdelantos.FirstOrDefaultAsync(p => p.IdTipoAdelanto == model.IdTipoAdelanto);
            var moneda = await _context.TipoMonedas.FirstOrDefaultAsync(p => p.IdTipoMoneda == model.IdTipoMoneda);
            
            //------------------------------------------------------------------------

            string folderPath = Path.Combine(_webHostEnvironment.ContentRootPath, "Mediaa");


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
                    file.CopyTo(fileStream);
                }

                Console.WriteLine($"Archivo ReferenciaOC guardado: {filePath}");
                model.ReferenciaOC = fileName;
            }

            foreach (var file in Proformacotizacion)
            {
                string fileName = file.FileName;
                string filePath = Path.Combine(folderPath, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                Console.WriteLine($"Archivo Proformacotizacion guardado: {filePath}");
                model.ProformaCotizacion = fileName;
            }

            foreach (var file in Factura)
            {
                string fileName = file.FileName;
                string filePath = Path.Combine(folderPath, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                Console.WriteLine($"Archivo Factura guardado: {filePath}");
                model.Factura = fileName;
            }


            if (model.LoginAprobador == 0)
            {
                model.LoginAprobador = 12;  // Valor por defecto
            }

            // Realiza las operaciones necesarias con el objeto "model"
            Console.WriteLine(model.ToJson());

            // Valida el modelo
            if (!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            // Asigna la fecha y hora actuales a FechaSolicitud
            model.FechaSolicitud = DateTime.Now;

            if (!string.IsNullOrEmpty(usuarioInfoJson))
            {
                Usuario usuario = JsonConvert.DeserializeObject<Usuario>(usuarioInfoJson);
                int? AreaUsuario = usuario.IdArea;

                // Crea una consulta de unión para combinar la información de las tablas AprobadorArea y Usuarios
                var aprobadoresArea = _context.AprobadorAreas
                    .Join(_context.Usuarios,
                        aprobador => aprobador.IdUsuario,
                        usuario => usuario.IdUsuario,
                        (aprobador, usuario) => new { Aprobador = aprobador, Usuario = usuario })
                    .Where(aprobadorUsuario => aprobadorUsuario.Aprobador.IdArea == AreaUsuario)
                    .ToList();

                // Filtra la lista de aprobadores para encontrar el que tiene la mayor capacidad de aprobación
                // que aún sea igual o menor al importe. Si no se encuentra ninguno, usa el IdAprobador = 49.
                var aprobador = aprobadoresArea
                .Where(aprobadorUsuario => aprobadorUsuario.Usuario.MontoAprobacion >= model.Importe)
                .OrderBy(aprobadorUsuario => aprobadorUsuario.Usuario.MontoAprobacion)
                .FirstOrDefault();


                if (aprobador != null)
                {
                    model.LoginAprobador = aprobador.Aprobador.IdUsuario;
                }
                else
                {
                    model.LoginAprobador = 49; // Asigna el valor 49 si no se encuentra un aprobador adecuado.
                }

            }

            // Obtiene el ID del usuario aprobador
            var aprobadorId = model.LoginAprobador;

            // Busca al usuario aprobador en la base de datos
            var aprobador1 = await _context.Usuarios.FindAsync(aprobadorId);

            // Si el usuario aprobador no se encuentra en la base de datos, devuelve un error
            if (aprobador1 == null)
            {
                return NotFound($"No se pudo encontrar un usuario con el ID {aprobadorId}");
            }

            // Obtiene el correo electrónico del usuario aprobador
            var correoAprobador = aprobador1.Correo;

            // Agrega el modelo a la base de datos
            var result = _context.Pagos.Add(model);

            await _context.SaveChangesAsync();
            string correo_emisor = "leedryk@gmail.com";
            string clave_emisor = "xxrlviitjlpqytrj";

            MailAddress receptor = new(correoAprobador);
            MailAddress emisor = new("leedryk@gmail.com");

            MailMessage email = new MailMessage(emisor, receptor);
            email.Subject = "Pruebas para SPP";

            string body = @"
            <div style='background-color: #F1F0E9; padding: 20px; width: 715px; text-align: center;'>
            <h2 style='font-weight: bold; font-size: 23px; color: #000000;'>SOLICITUD DE PAGO DE PROVEEDORES</h2>
            <div style='text-align: left; width: 666px; background: #F1F0E9; border: 2px solid #DDDAD2; padding: 10px;'>
                <label style='font-size: 15px; color: #000000;'>Número de Solicitud:</label>
                <br/>
                <div style='border: 1px solid #A79A66; width: 301px; height: 20px;'>"
                    + proveedor +
                @"</div>
                </div>
            </div>";

            email.Body = body;
            email.IsBodyHtml = true;  // Indicate that the email body is HTML

            SmtpClient smtp = new();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.Credentials = new NetworkCredential(correo_emisor, clave_emisor);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = true;

            smtp.Send(email);

            return Json(new { IdPago = result.Entity.IdPago });
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
            // Obtener la información del usuario actual
            string usuarioInfoJson = HttpContext.Request.Cookies["UsuarioInfo"];
            if (!string.IsNullOrEmpty(usuarioInfoJson))
            {
                Usuario usuarioActual = JsonConvert.DeserializeObject<Usuario>(usuarioInfoJson);
                int idUsuario = usuarioActual.IdUsuario;  // Aquí obtenemos el IdUsuario

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
                // Si no hay información del usuario en la cookie, puedes manejarlo de la manera que prefieras.
                // Por ejemplo, podrías redirigir al usuario a la página de inicio de sesión.
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