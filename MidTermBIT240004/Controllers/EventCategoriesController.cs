using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MidTermBIT240004.Data;

namespace MidTermBIT240004.Controllers
{
    public class EventCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.EventCategories_BIT240004.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound("EventCategoryId không tồn tại.");

            var eventCategory = await _context.EventCategories_BIT240004
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventCategory == null) return NotFound("EventCategoryId không tồn tại.");

            return View(eventCategory);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] EventCategory_BIT240004 eventCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eventCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(eventCategory);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound("EventCategoryId không tồn tại.");

            var eventCategory = await _context.EventCategories_BIT240004.FindAsync(id);
            if (eventCategory == null) return NotFound("EventCategoryId không tồn tại.");
            return View(eventCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] EventCategory_BIT240004 eventCategory)
        {
            if (id != eventCategory.Id) return NotFound("EventCategoryId không tồn tại.");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventCategoryExists(eventCategory.Id)) return NotFound("EventCategoryId không tồn tại.");
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(eventCategory);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound("EventCategoryId không tồn tại.");

            var eventCategory = await _context.EventCategories_BIT240004
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventCategory == null) return NotFound("EventCategoryId không tồn tại.");

            return View(eventCategory);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventCategory = await _context.EventCategories_BIT240004.FindAsync(id);
            if (eventCategory != null)
            {
                var hasEvents = await _context.Events_BIT240004.AnyAsync(e => e.EventCategoryId == id);
                if (hasEvents)
                {
                    ModelState.AddModelError("", "Không cho phép xóa loại sự kiện đang có sự kiện sử dụng.");
                    return View(eventCategory);
                }
                _context.EventCategories_BIT240004.Remove(eventCategory);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool EventCategoryExists(int id)
        {
            return _context.EventCategories_BIT240004.Any(e => e.Id == id);
        }
    }
}
