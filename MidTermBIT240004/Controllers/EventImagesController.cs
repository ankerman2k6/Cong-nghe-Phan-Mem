using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MidTermBIT240004.Data;

namespace MidTermBIT240004.Controllers
{
    public class EventImagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventImagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Create(int? eventId)
        {
            if (eventId == null) return NotFound("EventId không tồn tại.");
            ViewBag.EventId = eventId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ImageUrl,IsThumbnail,EventId")] EventImage_BIT240004 eventImage)
        {
            if (ModelState.IsValid)
            {
                if (eventImage.IsThumbnail)
                {
                    var existingThumbnails = await _context.EventImages_BIT240004
                        .Where(i => i.EventId == eventImage.EventId && i.IsThumbnail)
                        .ToListAsync();
                    
                    foreach (var th in existingThumbnails)
                    {
                        th.IsThumbnail = false;
                        _context.Update(th);
                    }
                }

                _context.Add(eventImage);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Events", new { id = eventImage.EventId });
            }
            ViewBag.EventId = eventImage.EventId;
            return View(eventImage);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetThumbnail(int id)
        {
            var eventImage = await _context.EventImages_BIT240004.FindAsync(id);
            if (eventImage == null) return NotFound("EventImageId không tồn tại.");

            var allImages = await _context.EventImages_BIT240004
                .Where(i => i.EventId == eventImage.EventId)
                .ToListAsync();

            foreach (var img in allImages)
            {
                img.IsThumbnail = img.Id == id;
                _context.Update(img);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Events", new { id = eventImage.EventId });
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound("EventImageId không tồn tại.");

            var eventImage = await _context.EventImages_BIT240004
                .Include(e => e.Event)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventImage == null) return NotFound("EventImageId không tồn tại.");

            return View(eventImage);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventImage = await _context.EventImages_BIT240004.FindAsync(id);
            if (eventImage != null)
            {
                _context.EventImages_BIT240004.Remove(eventImage);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Events", new { id = eventImage.EventId });
            }
            return RedirectToAction("Index", "Events");
        }
    }
}
