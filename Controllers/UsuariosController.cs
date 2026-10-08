using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using HelpDeskMvc.Data;
using HelpDeskMvc.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace HelpDeskMvc.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly AppDbContext _context;

    
        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }
        
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {

            
            var usuarios = await _context.Usuarios
                .OrderByDescending(u => u.DataCadastro)
                .ToListAsync();

            return View(usuarios);
        }

   
        public IActionResult Create()
        {
            return View(new MoltenUsuario());
        }

 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MoltenUsuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return View(usuario);
            }

            
            usuario.Id = 0;

            // Motra se o usuario ja ativou seu plano Molten
            usuario.StatusAssinatura = "Ativo";
            usuario.DataCadastro = DateTime.Now;

            // Verifica se o e-mail já existe no banco

            if (string.IsNullOrEmpty(usuario.Perfil))
            {
                usuario.Perfil = "Comum";
            }

           
            var emailExiste = await _context.Usuarios.AnyAsync(u => u.Email == usuario.Email);
            if (emailExiste)
            {
                ModelState.AddModelError("Email", "Este e-mail já está cadastrado no MOLTEN.");
                return View(usuario);
            }

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            
            return RedirectToAction(nameof(Login));
        }

        
        [HttpGet]
        public IActionResult Login()
        {
            
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View("LoginMolten"); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginMolten(string email, string senha)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(senha))
            {
                ViewBag.Erro = "Por favor, preencha todos os campos.";
                return View("LoginMolten");
            }

           
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email && u.Senha == senha);

            if (usuario == null)
            {
                ViewBag.Erro = "E-mail ou senha inválidos.";
                return View("LoginMolten");
            }

            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Perfil) 
            };

            var usuariosIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20)
            };

           
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(usuariosIdentity),
                authProperties);

            return RedirectToAction("Index", "Home");
        }

       
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }
    }
}
