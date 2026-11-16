using Microsoft.AspNetCore.Mvc;
using ASPCoreFirstApp.Models;

namespace ASPCoreFirstApp.Controllers;

public class GenreController : Controller
{
    private readonly ApplicationDbContext _db;

    public GenreController(ApplicationDbContext db)
    {
        _db = db;
    }

    // GET: /Genre/Index
    public IActionResult Index()
    {
        var genres = _db.Genres.ToList();
        return View(genres);
    }

    // GET: /Genre/Create (Displays the empty form)
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Genre/Create (Receives the submitted form and saves to DB)
    [HttpPost]
    [ValidateAntiForgeryToken] // Protects against CSRF / forged form submissions
    public IActionResult Create(Genre genre)
    {
        if (ModelState.IsValid)
        {
            genre.Id = Guid.NewGuid(); // Generates a unique ID for the Genre
            _db.Genres.Add(genre);
            _db.SaveChanges(); // Executes the SQL INSERT
            return RedirectToAction(nameof(Index));
        }
        return View(genre);
    }
}