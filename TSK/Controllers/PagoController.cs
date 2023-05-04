using Microsoft.AspNetCore.Mvc;

namespace SPP.Controllers
{
    public class PagoController : Controller
    {
        public IActionResult AdelantoPago()
        {
            @ViewBag.adelantopago = "active";
            return View();
        }
    }
}
