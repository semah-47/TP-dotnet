using Microsoft.AspNetCore.Mvc.Rendering;
using ASPCoreFirstApp.Models;

namespace ASPCoreFirstApp.ViewModels;

public class MovieDetailsViewModel
{
    public Movie Movie { get; set; } = null!;
    public List<MovieReview> Reviews { get; set; } = new();
    public double AverageRating { get; set; }
    public List<SelectListItem> Customers { get; set; } = new();
}