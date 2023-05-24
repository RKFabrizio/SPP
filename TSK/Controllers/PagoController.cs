using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TSK.Controllers
{
    [Authorize]
    public class PagoController : Controller
    {
        [Authorize]
        public IActionResult AdelantoPago()
        {

            @ViewBag.adelantopago = "active";
            return View();
        }
    }
}
