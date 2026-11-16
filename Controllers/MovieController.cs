using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASPCoreFirstApp.Models;
using ASPCoreFirstApp.Services;
using ASPCoreFirstApp.ViewModels;

namespace ASPCoreFirstApp.Controllers;

public class MovieController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly RentalDbContext _rentalDb;
    private readonly RentalService _rentalService;
    private readonly ReviewService _reviewService;

    public MovieController(
        ApplicationDbContext db,
        RentalDbContext rentalDb,
        RentalService rentalService,
        ReviewService reviewService)
    {
        _db = db;
        _rentalDb = rentalDb;
        _rentalService = rentalService;
        _reviewService = reviewService;
    }

    // GET: /Movie/Index?sortOrder=name_desc&page=1
    public async Task<IActionResult> Index(string? sortOrder, int page = 1)
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

        int totalItems = await moviesQuery.CountAsync();
        int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        if (page < 1) page = 1;
        if (page > totalPages && totalPages > 0) page = totalPages;

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = totalPages;

        var movies = await moviesQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return View(movies);
    }
    
    public IActionResult Create()
    {
        ViewBag.Genres = new SelectList(_db.Genres, "Id", "Name");
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Movie movie)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Genres = new SelectList(_db.Genres, "Id", "Name", movie.GenreId);
            return View(movie);
        }

        movie.IsAvailable = true;
        _db.Movies!.Add(movie);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    
    public async Task<IActionResult> Details(int id)
    {
        var movie = await _db.Movies!.Include(m => m.Genre).FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null) return NotFound();

        var reviews = await _reviewService.GetByMovieIdAsync(id);
        var avgRating = await _reviewService.GetAverageRatingAsync(id);
        var customers = await _rentalDb.Customers
            .AsNoTracking()
            .Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() })
            .ToListAsync();

        var vm = new MovieDetailsViewModel
        {
            Movie = movie,
            Reviews = reviews,
            AverageRating = avgRating,
            Customers = customers
        };

        return View(vm);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Rent(int movieId, int customerId)
    {
        var (success, errorMessage) = await _rentalService.RentMovieAsync(movieId, customerId);
        if (!success)
        {
            TempData["Error"] = errorMessage;
        }
        return RedirectToAction(nameof(Details), new { id = movieId });
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddReview(int movieId, string author, int rating, string? comment, string? tags, bool isSpoiler, string? language)
    {
        var review = new MovieReview
        {
            MovieId = movieId,
            Author = string.IsNullOrWhiteSpace(author) ? "Anonyme" : author,
            Rating = Math.Clamp(rating, 1, 5),
            Comment = comment,
            Tags = string.IsNullOrWhiteSpace(tags)
                ? new List<string>()
                : tags.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList(),
            IsSpoiler = isSpoiler ? true : null,
            Language = string.IsNullOrWhiteSpace(language) ? null : language
        };

        await _reviewService.AddReviewAsync(review);
        return RedirectToAction(nameof(Details), new { id = movieId });
    }
}