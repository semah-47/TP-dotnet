using ASPCoreFirstApp.Models;
using ASPCoreFirstApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ASPCoreFirstApp.Controllers;

public class MovieController : Controller
{
    private readonly ApplicationDbContext _db;

    public MovieController(ApplicationDbContext db)
    {
        _db = db;
    }
    
    
    public IActionResult Index(string? sortOrder, int page = 1)
    {
        int pageSize = 3;
        
        ViewBag.CurrentSort = sortOrder;
        ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        ViewBag.IdSortParm = sortOrder == "id_asc" ? "id_desc" : "id_asc";
        ViewBag.GenreSortParm = sortOrder == "genre_asc" ? "genre_desc" : "genre_asc";
        
        var moviesQuery = _db.Movies!.Include(m => m.Genre).AsQueryable();

        
        moviesQuery = sortOrder switch
        {
            "name_desc" => moviesQuery.OrderByDescending(m => m.Name),
            "id_asc" => moviesQuery.OrderBy(m => m.Id),
            "id_desc" => moviesQuery.OrderByDescending(m => m.Id),
            "genre_asc" => moviesQuery.OrderBy(m => m.Genre!.Name),
            "genre_desc" => moviesQuery.OrderByDescending(m => m.Genre!.Name),
            _ => moviesQuery.OrderBy(m => m.Name)
        };

        
        int totalItems = moviesQuery.Count();
        int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        if (page < 1) page = 1;
        if (page > totalPages && totalPages > 0) page = totalPages;

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = totalPages;

        
        var movies = moviesQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return View(movies);
    }

    public IActionResult Details(int id)
    {
        var movie = _db.Movies!.FirstOrDefault(m => m.Id == id);
        if (movie == null)
            return NotFound("Film introuvable !");

        return View(movie);
    }

    public IActionResult Create()
    {
        ViewBag.Genres = new SelectList(_db.Genres, "Id", "Name");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Movie movie)
    {
        if (ModelState.IsValid)
        {
            _db.Movies!.Add(movie);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        } 
        ViewBag.Genres = new SelectList(_db.Genres, "Id", "Name", movie.GenreId);
        return View(movie);
    }

    public IActionResult Edit(int id)
    {
        var movie = _db.Movies!.Find(id);
        if (movie == null) return NotFound();
        
        ViewBag.Genres = new SelectList(_db.Genres, "Id", "Name", movie.GenreId);
        return View(movie);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Movie movie)
    {
        if (id != movie.Id) return BadRequest();

        if (ModelState.IsValid)
        {
            _db.Update(movie);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Genres = new SelectList(_db.Genres, "Id", "Name", movie.GenreId);
        return View(movie);
    }

    public IActionResult Delete(int id)
    {
        var movie = _db.Movies!.Include(m => m.Genre).FirstOrDefault(m => m.Id == id);
        if (movie == null) return NotFound();
        
        return View(movie);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var movie = _db.Movies!.Find(id);
        if (movie != null)
        {
            _db.Movies.Remove(movie);
            _db.SaveChanges();
        }
        return RedirectToAction(nameof(Index));
    }
    
    [Route("Movie/released/{year}/{month}")]
    public IActionResult ByRelease(int year, int month)
    {
        return Content($"Attribute Routing -> Year: {year}, Month: {month}");
    }

    public IActionResult CustomerMovies()
    {
        var viewModel = new MovieCustomerViewModel
        {
            Customer = new Customer { Id = 1, Name = "Youssef Ben Ali" },
            Movies = new List<Movie>
            {
                new Movie { Id = 1, Name = "Inception" },
                new Movie { Id = 2, Name = "Interstellar" },
                new Movie { Id = 3, Name = "Dune: Part Two" }
            }
        };
        return View(viewModel);
    }

    public IActionResult CustomerDetails(int id)
    {
        var customers = new List<Customer>
        {
            new Customer { Id = 1, Name = "Youssef Ben Ali" },
            new Customer { Id = 2, Name = "Amira Trabelsi" },
            new Customer { Id = 3, Name = "Mehdi Gharbi" }
        };
        
        var customer = customers.FirstOrDefault(c => c.Id == id);
        
        if  (customer == null)
            return NotFound();
        
        return View(customer);
    }
}