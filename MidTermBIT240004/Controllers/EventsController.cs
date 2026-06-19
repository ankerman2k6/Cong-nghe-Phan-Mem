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
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchNameOrLocation, int? categoryId, DateTime? fromDate, DateTime? toDate, string sortOrder)
        {
            ViewData["searchNameOrLocation"] = searchNameOrLocation;
            ViewData["categoryId"] = categoryId;
            ViewData["fromDate"] = fromDate?.ToString("yyyy-MM-dd");
            ViewData["toDate"] = toDate?.ToString("yyyy-MM-dd");
            ViewData["sortOrder"] = sortOrder;

            ViewData["EventCategoryId"] = new SelectList(_context.EventCategories_BIT240004, "Id", "Name", categoryId);

            IQueryable<Event_BIT240004> events = _context.Events_BIT240004
                .Include(e => e.EventCategory)
                .Include(e => e.EventImages);

            if (!string.IsNullOrEmpty(searchNameOrLocation))
            {
                events = events.Where(e => e.Name.Contains(searchNameOrLocation) || e.Location.Contains(searchNameOrLocation));
            }

            if (categoryId.HasValue)
            {
                events = events.Where(e => e.EventCategoryId == categoryId.Value);
            }

            if (fromDate.HasValue)
            {
                events = events.Where(e => e.StartDate >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                events = events.Where(e => e.StartDate <= toDate.Value);
            }

            // Client evaluation for closest date using ToListAsync then sort?
            // "Không được lấy toàn bộ dữ liệu rồi lọc bằng vòng lặp" .
            
            switch (sortOrder)
            {
                case "closest_date":
                    var now = DateTime.Now;
                    events = events.OrderBy(e => Math.Abs(EF.Functions.DateDiffSecond(now, e.StartDate)));
                    break;
                case "price_asc":
                    events = events.OrderBy(e => e.Price);
                    break;
                case "price_desc":
                    events = events.OrderByDescending(e => e.Price);
                    break;
                default:
                    events = events.OrderByDescending(e => e.StartDate);
                    break;
            }

            var model = await events.ToListAsync();
            
            if (model.Count == 0 && (searchNameOrLocation != null || categoryId != null || fromDate != null || toDate != null))
            {
                ViewBag.Message = "Tìm kiếm không có kết quả.";
            }

            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound("EventId không tồn tại.");

            var ev = await _context.Events_BIT240004
                .Include(e => e.EventCategory)
                .Include(e => e.EventImages)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (ev == null) return NotFound("EventId không tồn tại.");

            return View(ev);
        }

        public IActionResult Create()
        {
            ViewData["EventCategoryId"] = new SelectList(_context.EventCategories_BIT240004, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,StartDate,EndDate,Location,Description,EventCategoryId")] Event_BIT240004 ev)
        {
            if (ev.StartDate > ev.EndDate)
            {
                ModelState.AddModelError("EndDate", "Ngày kết thúc không hợp lệ.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(ev);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventCategoryId"] = new SelectList(_context.EventCategories_BIT240004, "Id", "Name", ev.EventCategoryId);
            return View(ev);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound("EventId không tồn tại.");

            var ev = await _context.Events_BIT240004.FindAsync(id);
            if (ev == null) return NotFound("EventId không tồn tại.");

            ViewData["EventCategoryId"] = new SelectList(_context.EventCategories_BIT240004, "Id", "Name", ev.EventCategoryId);
            return View(ev);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,StartDate,EndDate,Location,Description,EventCategoryId")] Event_BIT240004 ev)
        {
            if (id != ev.Id) return NotFound("EventId không tồn tại.");

            if (ev.StartDate > ev.EndDate)
            {
                ModelState.AddModelError("EndDate", "Ngày kết thúc không hợp lệ.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ev);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(ev.Id)) return NotFound("EventId không tồn tại.");
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventCategoryId"] = new SelectList(_context.EventCategories_BIT240004, "Id", "Name", ev.EventCategoryId);
            return View(ev);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound("EventId không tồn tại.");

            var ev = await _context.Events_BIT240004
                .Include(e => e.EventCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (ev == null) return NotFound("EventId không tồn tại.");

            return View(ev);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ev = await _context.Events_BIT240004.FindAsync(id);
            if (ev != null)
            {
                var now = DateTime.Now;
                if (ev.StartDate <= now && ev.EndDate >= now)
                {
                    ModelState.AddModelError("", "Không cho phép xóa sự kiện đang diễn ra.");
                    ev.EventCategory = await _context.EventCategories_BIT240004.FindAsync(ev.EventCategoryId);
                    return View(ev);
                }

                _context.Events_BIT240004.Remove(ev);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Events_BIT240004.Any(e => e.Id == id);
        }
    }
}
