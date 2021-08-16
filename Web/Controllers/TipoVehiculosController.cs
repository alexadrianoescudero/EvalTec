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
    public class TipoVehiculosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TipoVehiculosController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Admin,User")]
        // GET: TipoVehiculos
        public async Task<IActionResult> Index()
        {
            return View(await _context.TipoVehiculos.ToListAsync());
        }
        [Authorize(Roles = "Admin,User")]
        // GET: TipoVehiculos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoVehiculo = await _context.TipoVehiculos
                .FirstOrDefaultAsync(m => m.Codigo == id);
            if (tipoVehiculo == null)
            {
                return NotFound();
            }

            return View(tipoVehiculo);
        }
        [Authorize(Roles = "Admin")]
        // GET: TipoVehiculos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoVehiculos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( TipoVehiculo tipoVehiculo)
        {
            if (ModelState.IsValid)
            {
                tipoVehiculo.Estado = 1;
                _context.Add(tipoVehiculo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoVehiculo);
        }
        [Authorize(Roles = "Admin")]
        // GET: TipoVehiculos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoVehiculo = await _context.TipoVehiculos.FindAsync(id);
            if (tipoVehiculo == null)
            {
                return NotFound();
            }
            return View(tipoVehiculo);
        }

        // POST: TipoVehiculos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TipoVehiculo tipoVehiculo)
        {
            if (id != tipoVehiculo.Codigo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoVehiculo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoVehiculoExists(tipoVehiculo.Codigo))
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
            return View(tipoVehiculo);
        }
        [Authorize(Roles = "Admin")]
        // GET: TipoVehiculos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoVehiculo = await _context.TipoVehiculos
                .FirstOrDefaultAsync(m => m.Codigo == id);
            if (tipoVehiculo == null)
            {
                return NotFound();
            }

            return View(tipoVehiculo);
        }

        /// <summary>
        /// Accion que estado a las parqueo
        /// </summary>
        /// <param name="id">Codigo de la cuenta </param>
        /// <returns>Activacion de la cuenta</returns>        
        [Authorize(Roles = "Admin")]
        public ActionResult Activar(string id)
        {
            TipoVehiculo tipoVehiculo = _context.TipoVehiculos.Where(x => x.Codigo == Convert.ToInt32(id)).FirstOrDefault();
            tipoVehiculo.Estado = 1;
            _context.Update(tipoVehiculo);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Desactivar(string id)
        {
            TipoVehiculo tipoVehiculo = _context.TipoVehiculos.Where(x => x.Codigo == Convert.ToInt32(id)).FirstOrDefault();
            tipoVehiculo.Estado = 0;
            _context.Update(tipoVehiculo);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        private bool TipoVehiculoExists(int id)
        {
            return _context.TipoVehiculos.Any(e => e.Codigo == id);
        }
    }
}
