using Microsoft.AspNetCore.Mvc;

namespace TSK.Controllers
{
    public class TokenUsuario : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Aprobador()
        {
            @ViewBag.aprobador = "active";
            return View();
        }
    }
}
