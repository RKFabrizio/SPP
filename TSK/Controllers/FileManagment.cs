using Microsoft.AspNetCore.Mvc;
using DevExtreme.AspNet.Mvc.FileManagement;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace TSK.Controllers
{
    [Route("api/[controller]/[action]")]
    public class FileManagmentController : Controller
    {

        private string _filesDirectory = @"C:\Users\esteb\Desktop\New folder\SPP\TSK\Mediaa\"; // Asegúrate de reemplazar este valor con el directorio de archivos en tu servidor.

        public ActionResult FileManagment()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadRef(List<IFormFile> ReferenciaOC)
        {
            foreach (var file in ReferenciaOC)
            {
                if (file.Length > 0)
                {
                    string fileName = file.FileName;
                    string filePath = Path.Combine("ruta_del_directorio_de_guardado", fileName); // Especifica la ruta donde deseas guardar los archivos adjuntos
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
            }

            // Realiza cualquier otra operación necesaria

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UploadProforma(List<IFormFile> Proformacotizacion)
        {
            foreach (var file in Proformacotizacion)
            {
                if (file.Length > 0)
                {
                    string fileName = file.FileName;
                    string filePath = Path.Combine("ruta_del_directorio_de_guardado", fileName); // Especifica la ruta donde deseas guardar los archivos adjuntos
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
            }

            // Realiza cualquier otra operación necesaria

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFactura(List<IFormFile> Factura)
        {
            foreach (var file in Factura)
            {
                if (file.Length > 0)
                {
                    string fileName = file.FileName;
                    string filePath = Path.Combine("ruta_del_directorio_de_guardado", fileName); // Especifica la ruta donde deseas guardar los archivos adjuntos
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
            }

            // Realiza cualquier otra operación necesaria

            return Ok();
        }

    }

}