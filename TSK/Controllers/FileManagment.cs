using Microsoft.AspNetCore.Mvc;

namespace TSK.Controllers
{
    public class FileManagment : Controller
    {
    
            private string _filesDirectory = @"C:\Users\fabri\OneDrive\Escritorio\Uploads"; // Asegúrate de reemplazar este valor con el directorio de archivos en tu servidor.

            [HttpPost]
            public ActionResult UploadRef(List<IFormFile> ReferenciaOC)
            {
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
