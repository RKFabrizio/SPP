using Microsoft.AspNetCore.Mvc;

namespace TSK.Controllers
{
    public class TokenUsuario : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("TokenUsuario/Aprobador/{parametro1}/{parametro2}")]
        public IActionResult Aprobador(string parametro1, string parametro2)
        {
            ViewBag.aprobador = "active";
            ViewBag.parametro1 = parametro1;
            ViewBag.parametro2 = parametro2;
            return View();
        }
    }
}
