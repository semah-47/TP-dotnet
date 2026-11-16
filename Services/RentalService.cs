using ASPCoreFirstApp.Models;

namespace ASPCoreFirstApp.Services;

public class RentalService
{
    private readonly ApplicationDbContext _catalogDb;
    private readonly RentalDbContext _rentalDb;

    public RentalService(ApplicationDbContext catalogDb, RentalDbContext rentalDb)
    {
        _catalogDb = catalogDb;
        _rentalDb = rentalDb;
    }

    public async Task<(bool Success, string? ErrorMessage)> RentMovieAsync(int movieId, int customerId)
    {
        var movie = await _catalogDb.Movies!.FindAsync(movieId);
        if (movie == null)
            return (false, "Film introuvable.");
        if (!movie.IsAvailable)
            return (false, "Ce film est déjà loué.");

        var customerExists = await _rentalDb.Customers.FindAsync(customerId) != null;
        if (!customerExists)
            return (false, "Client introuvable.");
        
        movie.IsAvailable = false;
        await _catalogDb.SaveChangesAsync();

        try
        {
            var rental = new Rental
            {
                MovieId = movieId,
                CustomerId = customerId,
                RentedAt = DateTime.UtcNow
            };
            _rentalDb.Rentals.Add(rental);
            await _rentalDb.SaveChangesAsync();

            return (true, null);
        }
        catch (Exception ex)
        {
            movie.IsAvailable = true;
            await _catalogDb.SaveChangesAsync();
            return (false, $"Échec de la location (compensation appliquée) : {ex.Message}");
        }
    }
}