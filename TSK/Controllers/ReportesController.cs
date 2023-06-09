﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SPP.Controllers
{
    [Authorize]
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
        public IActionResult PendienteAprobarxAprobador()
        {
            @ViewBag.rep = "active";
            @ViewBag.pendienteaprobarxaprobador = "active";
            return View();
        }
    }
}
