using MongoDB.Driver;
using ASPCoreFirstApp.Models;

namespace ASPCoreFirstApp.Services;

public class ReviewService
{
    private readonly IMongoCollection<MovieReview> _reviews;

    public ReviewService(IMongoDatabase database)
    {
        _reviews = database.GetCollection<MovieReview>("Reviews");
    }

    public async Task<List<MovieReview>> GetByMovieIdAsync(int movieId)
    {
        return await _reviews
            .Find(r => r.MovieId == movieId)
            .SortByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task AddReviewAsync(MovieReview review)
    {
        await _reviews.InsertOneAsync(review);
    }

    public async Task<double> GetAverageRatingAsync(int movieId)
    {
        var reviews = await GetByMovieIdAsync(movieId);
        return reviews.Count == 0 ? 0 : Math.Round(reviews.Average(r => r.Rating), 1);
    }
}