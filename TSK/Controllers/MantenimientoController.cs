using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SPP.Controllers
{
    [Authorize]
    public class MantenimientoController : Controller
    {
        public IActionResult Area()
        {
            @ViewBag.mantenimiento = "active";
            @ViewBag.area = "active";
            return View();
        }
        public IActionResult Usuario()
        {
            @ViewBag.mantenimiento = "active";
            @ViewBag.usuario = "active";
            return View();
        }
        public IActionResult Correo()
        {
            @ViewBag.mantenimiento = "active";
            @ViewBag.correo = "active";
            return View();
        }
        public IActionResult Aprobador()
        {
            @ViewBag.mantenimiento = "active";
            @ViewBag.aprobador = "active";
            return View();
        }
        public IActionResult Compania()
        {
            @ViewBag.mantenimiento = "active";
            @ViewBag.compania = "active";
            return View();
        }
        public IActionResult Proveedor()
        {
            @ViewBag.mantenimiento = "active";
            @ViewBag.proveedor = "active";
            return View();
        }

        public IActionResult Banco()
        {
            @ViewBag.mantenimiento = "active";
            @ViewBag.banco = "active";
            return View();
        }
        public IActionResult Contacto()
        {
            @ViewBag.mantenimiento = "active";
            @ViewBag.contacto = "active";
            return View();
        }
    }
}
