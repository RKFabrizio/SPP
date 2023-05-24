using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SPP.Controllers
{
    public class AdministracionController : Controller
    {
        [Authorize]
        public IActionResult ListaSolicitudes()
        {
            @ViewBag.adm = "active";
            @ViewBag.listasolicitud = "active";
            return View();
        }
        public IActionResult PendientePago()
        {
            @ViewBag.adm = "active";
            @ViewBag.pendientepago = "active";
            return View();
        }
    }
}
