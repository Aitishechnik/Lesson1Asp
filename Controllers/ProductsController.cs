using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lesson1.Data;
using Lesson1.Models;
using Microsoft.IdentityModel.Tokens;
using Lesson1.Middleware;

namespace Lesson1.Controllers
{
    public class ProductsController : Controller, ILoggerCustom<Product>
    {
        private readonly Lesson1Context _context;

        public Middleware.Logger<Product> Logger { get; set; } = new Middleware.Logger<Product>();

        public ProductsController(Lesson1Context context)
        {
            _context = context;
        }

        //Написать экшн который принимает на вход число, выводит список товаров, которые цена меньше введенной цифры

        // GET: Products 
        public async Task<IActionResult> Index()
        {
            var products = await _context.Product.ToListAsync();

            GetLowHighPrices(products);

            return View(products);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.ID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Calories,Shelflife,Category,Description,Price")] Product product)
        {
            if (Program.currentUser.HasPermission(PermissionEntity.Product, PermissionRight.Create))
            {

                if (ModelState.IsValid)
                {
                    _context.Add(product);
                    await _context.SaveChangesAsync();
                    Log(product, Program.currentUser, LogAction.Create);
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            Log(product, Program.currentUser, LogAction.Update);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Calories,Shelflife,Category,Description,Price")] Product product)
        {
            if (Program.currentUser.HasPermission(PermissionEntity.Product, PermissionRight.Update))
            {
                if (id != product.ID)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(product);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ProductExists(product.ID))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            Log(product, Program.currentUser, LogAction.Update);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.ID == id);
            if (product == null)
            {
                return NotFound();
            }
            Log(product, Program.currentUser, LogAction.Delete);
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (Program.currentUser.HasPermission(PermissionEntity.Product, PermissionRight.Delete))
            {
                var product = await _context.Product.FindAsync(id);
                if (product != null)
                {
                    _context.Product.Remove(product);
                }
                Log(product, Program.currentUser, LogAction.Delete);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Filter(int? decreasingValue)
        {
            var products = await _context.Product.ToListAsync();
            GetLowHighPrices(products);
            if (decreasingValue.ToString().IsNullOrEmpty() || decreasingValue == 0)
            {
                return View("Index", products);
            }

            List<Product> result = new List<Product>();
            List<float> VATs = new List<float>();

            foreach (var item in products)
            {
                VATs.Add(item.Price * 0.2f);

                if (item.Price < decreasingValue)
                    result.Add(item);
            }

            ViewBag.VATs = VATs;
            return View("Index", result);
        }

        private void GetLowHighPrices(List<Product> products)
        {
            ViewBag.MinPrice = int.MaxValue;
            ViewBag.MaxPrice = 0;           

            foreach (var product in products)
            {
                if (ViewBag.MinPrice > product.Price)
                    ViewBag.MinPrice = product.Price;
                if (ViewBag.MaxPrice < product.Price)
                    ViewBag.MaxPrice = product.Price;
            }
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ID == id);
            
        }

        public void Log<T>(T entity, User user, LogAction logAction)
        {
            System.IO.File.AppendAllText(ILoggerCustom<Product>.FileAdress, Logger.PrepareLogText(entity as Product, user, logAction));
        }
    }
}
