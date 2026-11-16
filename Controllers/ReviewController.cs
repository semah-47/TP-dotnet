using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ASPCoreFirstApp.Models;

namespace ASPCoreFirstApp.Controllers;

public class ReviewController : Controller
{
    private readonly IMongoCollection<MovieReview> _reviews;

    public ReviewController(IMongoDatabase database)
    {
        _reviews = database.GetCollection<MovieReview>("Reviews");
    }
    
    public async Task<IActionResult> ByMovie(int movieId)
    {
        var list = await _reviews.Find(r => r.MovieId == movieId).ToListAsync();
        return Json(list);
    }
    
    [HttpPost]
    public async Task<IActionResult> Add(MovieReview review)
    {
        await _reviews.InsertOneAsync(review);
        return Ok(review);
    }
}