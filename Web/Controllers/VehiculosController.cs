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
    public class VehiculosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VehiculosController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Admin,User")]
        // GET: Vehiculoes
        public async Task<IActionResult> Index()
        {
            var parqueaderoContext = _context.Vehiculos.Include(v => v.CedulaClienteNavigation).Include(v => v.TipoVehiculoNavigation);
            return View(await parqueaderoContext.ToListAsync());
        }
        [Authorize(Roles = "Admin,User")]
        // GET: Vehiculoes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehiculo = await _context.Vehiculos
                .Include(v => v.CedulaClienteNavigation)
                .Include(v => v.TipoVehiculoNavigation)
                .FirstOrDefaultAsync(m => m.Placa == id);
            if (vehiculo == null)
            {
                return NotFound();
            }

            return View(vehiculo);
        }

        // GET: Vehiculoes/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["CedulaCliente"] = new SelectList(_context.Clientes, "Cedula", "Cedula");
            ViewData["TipoVehiculo"] = new SelectList(_context.TipoVehiculos, "Codigo", "Descripcion");
            return View();
        }

        // POST: Vehiculoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Vehiculo vehiculo)
        {
            if (ModelState.IsValid)
            {
                vehiculo.Estado = 1;
                _context.Add(vehiculo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CedulaCliente"] = new SelectList(_context.Clientes, "Cedula", "Cedula", vehiculo.CedulaCliente);
            ViewData["TipoVehiculo"] = new SelectList(_context.TipoVehiculos, "Codigo", "Descripcion", vehiculo.TipoVehiculo);
            return View(vehiculo);
        }
        [Authorize(Roles = "Admin")]
        // GET: Vehiculoes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehiculo = await _context.Vehiculos.FindAsync(id);
            if (vehiculo == null)
            {
                return NotFound();
            }
            ViewData["CedulaCliente"] = new SelectList(_context.Clientes, "Cedula", "Cedula", vehiculo.CedulaCliente);
            ViewData["TipoVehiculo"] = new SelectList(_context.TipoVehiculos, "Codigo", "Descripcion", vehiculo.TipoVehiculo);
            return View(vehiculo);
        }

        // POST: Vehiculoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Placa,Chasis,Estado,TipoVehiculo,CedulaCliente")] Vehiculo vehiculo)
        {
            if (id != vehiculo.Placa)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehiculo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehiculoExists(vehiculo.Placa))
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
            ViewData["CedulaCliente"] = new SelectList(_context.Clientes, "Cedula", "Cedula", vehiculo.CedulaCliente);
            ViewData["TipoVehiculo"] = new SelectList(_context.TipoVehiculos, "Codigo", "Descripcion", vehiculo.TipoVehiculo);
            return View(vehiculo);
        }

       

        /// <summary>
        /// Accion que estado a las parqueo
        /// </summary>
        /// <param name="id">Codigo de la cuenta </param>
        /// <returns>Activacion de la cuenta</returns>        
        [Authorize(Roles = "Admin")]
        public ActionResult Activar(string id)
        {
            Vehiculo vehiculo = _context.Vehiculos.Where(x => x.Placa == id).FirstOrDefault();
            vehiculo.Estado = 1;
            _context.Update(vehiculo);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Desactivar(string id)
        {
            Vehiculo vehiculo = _context.Vehiculos.Where(x => x.Placa == id).FirstOrDefault();
            vehiculo.Estado = 0;
            _context.Update(vehiculo);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        private bool VehiculoExists(string id)
        {
            return _context.Vehiculos.Any(e => e.Placa == id);
        }
    }
}
