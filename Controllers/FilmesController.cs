using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HelpDeskMvc.Data;
using HelpDeskMvc.Models;
using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HelpDeskMvc.Controllers
{
    public class FilmesController : Controller
    {
        private readonly AppDbContext _context;

        public FilmesController(AppDbContext context)
        {
            _context = context;
        }

       
        public async Task<IActionResult> Index()
        {
            var filmes = await _context.Filmes
                .OrderByDescending(f => f.DataAbertura)
                .ToListAsync();

            return View(filmes);
        }

        
        public async Task<IActionResult> Detalhes(int id)
        {
            var filmeRecuperado = await _context.Filmes
                .FirstOrDefaultAsync(f => f.Id == id);

            if (filmeRecuperado == null)
            {
                return NotFound();
            }

            ViewData["Title"] = "Detalhes dos Filmes";

            return View(filmeRecuperado);
        }

        
        [Authorize(Roles = "Admin")] 
        public IActionResult Create()
        {
            return View(new Filme());
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> Create(Filme filme)
        {
            if (!ModelState.IsValid)
            {
                return View(filme);
            }

            filme.Id = 0;
            filme.Status = "Nos Cinemas";
            filme.DataAbertura = DateTime.Now;
            filme.DataFechamento = null;

            _context.Filmes.Add(filme);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
