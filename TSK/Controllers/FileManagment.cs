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

        private string _filesDirectory = @"C:\Users\esteb\Desktop\New folder\SPP\TSK\wwwroot\media\"; // Asegúrate de reemplazar este valor con el directorio de archivos en tu servidor.

        public ActionResult FileManagment()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UploadRef(List<IFormFile> ReferenciaOC)
        {
            System.Console.WriteLine("Test111111111111111111111111111111111111111111");
            foreach (var file in ReferenciaOC)
            {
                var filePath = Path.Combine(_filesDirectory, file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }

            return Json(new { success = true });
        }



        [HttpPost]
        public ActionResult UploadProforma(List<IFormFile> Proformacotizacion)
        {
            foreach (var file in Proformacotizacion)
            {
                var filePath = Path.Combine(_filesDirectory, file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult UploadFactura(List<IFormFile> Factura)
        {
            foreach (var file in Factura)
            {
                var filePath = Path.Combine(_filesDirectory, file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult DeleteFile(string fileName)
        {
            var filePath = Path.Combine(_filesDirectory, fileName);

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            return Json(new { success = true });
        }
    }

}
