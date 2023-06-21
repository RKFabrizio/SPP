using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TSK.Controllers
{
    [Authorize]
    public class ContactoController : Controller
    {
        [Authorize]
        public IActionResult ContactoUsuario()
        {
            @ViewBag.contactousuario = "active";
            return View();
        }
    }
}
