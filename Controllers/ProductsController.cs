using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lesson1.Data;
using Lesson1.Models;

namespace Lesson1.Controllers
{
    public class ProductsController : Controller
    {
        private readonly Lesson1Context _context;

        public ProductsController(Lesson1Context context)
        {
            _context = context;
        }

        //Написать экшн который принимает на вход число, выводит список товаров, которые цена меньше введенной цифры


        // GET: Products/Filter/5
        public async Task<IActionResult> Filter(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var products = await _context.Product.ToListAsync();

            List<Product> result = new List<Product>();
            List<float> VATs = new List<float>();

            foreach(var item in products)
            {
                VATs.Add(item.Price * 0.2f);

                if (item.Price < id)
                    result.Add(item);
            }

            ViewBag.VATs = VATs;
            return View(result);
        }


        // GET: Products 
        public async Task<IActionResult> Index()
        {
            var products = await _context.Product.ToListAsync();

            ViewBag.sum = 0;
            foreach(var pr in products)
            {
                ViewBag.sum += pr.Calories;
                ViewBag.randomTexts += pr.Name + "\n";
            }
                

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
            if (Program.currentUser.HasPermission(PermissionEntity.Product, PermissionRight.Delete))
            {

                if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
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
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Calories,Shelflife,Category,Description,Price")] Product product)
        {
            if (Program.currentUser.HasPermission(PermissionEntity.Product, PermissionRight.Delete))
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

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if(Program.currentUser.HasPermission(PermissionEntity.Product, PermissionRight.Delete))
            {
                var product = await _context.Product.FindAsync(id);
                if (product != null)
                {
                    _context.Product.Remove(product);
                }

                await _context.SaveChangesAsync();
            }        
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ID == id);
        }
    }
}
