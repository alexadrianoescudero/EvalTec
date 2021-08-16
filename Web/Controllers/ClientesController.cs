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
    public class ClientesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientesController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Admin,User")]
        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Clientes.ToListAsync());
        }
        [Authorize(Roles = "Admin,User")]
        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Cedula == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }
        [Authorize(Roles = "Admin")]
        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                cliente.Estado = 1;
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }
        [Authorize(Roles = "Admin")]
        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Cliente cliente)
        {
            if (id != cliente.Cedula)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.Cedula))
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
            return View(cliente);
        }      

        /// <summary>
        /// Accion que estado las cliente
        /// </summary>
        /// <param name="id">Codigo de la cuenta </param>
        /// <returns>Activacion de la cuenta</returns>        
        [Authorize(Roles = "Admin")]     
        public ActionResult Activar(string id)
        {
            Cliente cliente = _context.Clientes.Where(x => x.Cedula == id).FirstOrDefault();
            cliente.Estado = 1;
            _context.Update(cliente);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Desactivar(string id)
        {
            Cliente cliente = _context.Clientes.Where(x => x.Cedula == id).FirstOrDefault();
            cliente.Estado = 0;
            _context.Update(cliente);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        private bool ClienteExists(string id)
        {
            return _context.Clientes.Any(e => e.Cedula == id);
        }
    }
}
