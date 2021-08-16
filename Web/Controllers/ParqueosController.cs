using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Datos.Data;
using Datos.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Web.Controllers
{
    [Authorize]
    public class ParqueosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ParqueosController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Admin,User")]
        // GET: Parqueos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Parqueos.ToListAsync());
        }
        [Authorize(Roles = "Admin,User")]
        // GET: Parqueos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parqueo = await _context.Parqueos
                .FirstOrDefaultAsync(m => m.Codigo == id);
            if (parqueo == null)
            {
                return NotFound();
            }

            return View(parqueo);
        }
        [Authorize(Roles = "Admin")]
        // GET: Parqueos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Parqueos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Parqueo parqueo)
        {
            if (ModelState.IsValid)
            {
                parqueo.Estado = 1;
                _context.Add(parqueo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(parqueo);
        }
        [Authorize(Roles = "Admin")]
        // GET: Parqueos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parqueo = await _context.Parqueos.FindAsync(id);
            if (parqueo == null)
            {
                return NotFound();
            }
            return View(parqueo);
        }

        // POST: Parqueos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  Parqueo parqueo)
        {
            if (id != parqueo.Codigo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(parqueo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParqueoExists(parqueo.Codigo))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(parqueo);
        }       

        /// <summary>
        /// Accion que estado a las parqueo
        /// </summary>
        /// <param name="id">Codigo de la cuenta </param>
        /// <returns>Activacion de la cuenta</returns>        
        [Authorize(Roles = "Admin")]
        public ActionResult Activar(string id)
        {
            Parqueo parqueo = _context.Parqueos.Where(x => x.Codigo ==Convert.ToInt32( id)).FirstOrDefault();
            parqueo.Estado = 1;
            _context.Update(parqueo);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Desactivar(string id)
        {
            Parqueo parqueo = _context.Parqueos.Where(x => x.Codigo == Convert.ToInt32(id)).FirstOrDefault();
            parqueo.Estado = 0;
            _context.Update(parqueo);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        private bool ParqueoExists(int id)
        {
            return _context.Parqueos.Any(e => e.Codigo == id);
        }
    }
}
