using System.ComponentModel.DataAnnotations;

namespace ASPCoreFirstApp.Models;

public class Movie
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Le titre du film est obligatoire.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Le titre doit contenir entre 2 et 100 caractères.")]
    public string Name { get; set; } = null!;

    public bool IsAvailable { get; set; } = true;

    public Guid? GenreId { get; set; }
    public Genre? Genre { get; set; }

    public Movie() { }
}