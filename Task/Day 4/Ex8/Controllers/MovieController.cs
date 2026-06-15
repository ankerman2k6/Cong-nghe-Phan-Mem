using System.Linq;
using Ex8.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ex8.Controllers
{
    public class MovieController : Controller
    {
        private static List<Movie> movies;
        
        public IActionResult Index()
        {
            ViewBag.TotalMovies = movies.Count;
            ViewBag.PageTitle = "Danh sách phim";
            return View(movies); //Truyền danh sách phim vào view để hiển thị

        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Movie movie)
        {
            if (ModelState.IsValid)
            {
                movie.Id = movies.Count + 1;
                movies.Add(movie);
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        //Xem chi tiết 1 phim
        public IActionResult Details(int id)
        {
            ViewBag.PageTitle = "Chi tiết Phim";
            var movie = movies.FirstOrDefault(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }
            ViewBag.NameMovie = movie.Title;
            return View(movie);
        }

        //Edit 1 phim
        public IActionResult Edit(int id)
        {
            var movie = movies.FirstOrDefault(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie); // Truyền thông tin phim vào view để hiển thị form edit
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //URL: POST /Movie/Edit/1
        public IActionResult Edit(int id, Movie movie)
        {
            if(id != movie.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var existingMovie = movies.FirstOrDefault(movie => movie.Id == id);
                if (existingMovie == null)
                {
                    return NotFound();
                }

                //Cập nhật thông tin
                existingMovie.Title = movie.Title;
                existingMovie.PublishYear = movie.PublishYear;
                existingMovie.Description = movie.Description;
                existingMovie.National = movie.National;

                TempData["SuccessMessage"] = "Cập nhật phim thành công!";
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        //Xóa 1 phim
        public IActionResult Delete(int id)
        {
            var movie = movies.FirstOrDefault (m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie); //Hiển thị trang xác nhận xoá với thông tin phim
        }

        //URL: POST /Movie/Delete/5 DELETE
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var movie = movies.FirstOrDefault(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }
            movies.Remove(movie);
            TempData["SuccessMessage"] = $"Xóa phim {movie.Title} thành công!";
            return RedirectToAction("Index");
        }


    }
}