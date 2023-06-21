using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TSK.Controllers
{
 
    public class ContactoController : Controller
    {
 
        public IActionResult ContactoUsuario()
        {
            @ViewBag.contactousuario = "active";
            return View();
        }
    }
}
