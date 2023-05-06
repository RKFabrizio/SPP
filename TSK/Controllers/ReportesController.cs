using Microsoft.AspNetCore.Mvc;

namespace SPP.Controllers
{
    public class ReportesController : Controller
    {
        public IActionResult MiSolicitud()
        {
            @ViewBag.rep = "active";
            @ViewBag.misolicitud = "active";
            return View();
        }
        public IActionResult PendienteAprobar()
        {
            @ViewBag.rep = "active";
            @ViewBag.pendienteaprobar = "active";
            return View();
        }
    }
}
