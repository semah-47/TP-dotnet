using System.ComponentModel.DataAnnotations;

namespace ASPCoreFirstApp.Models;

public class Customer
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Le nom du client est obligatoire.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Le nom doit contenir entre 2 et 100 caractères.")]
    public string Name { get; set; } = null!;

    [EmailAddress(ErrorMessage = "L'adresse email saisie n'est pas valide.")]
    public string? Email { get; set; }

    public int MembershipTypeId { get; set; }
    public MembershipType? MembershipType { get; set; }

    public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
}