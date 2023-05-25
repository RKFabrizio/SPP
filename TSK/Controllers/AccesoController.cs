using Microsoft.AspNetCore.Mvc;
using SPP.Data;
using SPP.Models;
using System.Text;
using System.Security.Cryptography;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Newtonsoft.Json;

namespace TSK.Controllers
{
    public class AccesoController : Controller
    {

        UsuarioDatos _UsuarioDatos = new UsuarioDatos();
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Usuario _usuario)
        {
            var usuario = _UsuarioDatos.ValidarUsuario(_usuario.Login, _usuario.Contrasena);

            if (usuario != null && usuario.Habilitado)
            {
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, usuario.Nombre),
            new Claim("Usuario", usuario.Login),
            new Claim("NOMBREPERFIL", usuario.Perfiles[0])
        };

                foreach (string per in usuario.Perfiles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, per));
                };

                claims.Add(new Claim("UsuarioInfo", JsonConvert.SerializeObject(usuario)));

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                // Aquí es donde se guarda la información del usuario en una cookie
                string usuarioInfoJson = JsonConvert.SerializeObject(usuario);
                var cookieOptions = new CookieOptions() { IsEssential = true };
                HttpContext.Response.Cookies.Append("UsuarioInfo", usuarioInfoJson, cookieOptions);

                return RedirectToAction("Bienvenida", "Inicio");
            }
            else
            {
                TempData["Error"] = "Usuario o contraseña incorrectos";
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Acceso");
        }



    }





}