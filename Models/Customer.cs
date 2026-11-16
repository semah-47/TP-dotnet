using System.ComponentModel.DataAnnotations;

namespace ASPCoreFirstApp.Models;

public class Customer
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Le nom est obligatoire."), StringLength(100)]
    public string Name { get; set; } = "";

    [EmailAddress(ErrorMessage = "Adresse email invalide.")]
    public string? Email { get; set; }

    public int MembershipTypeId { get; set; }
    public MembershipType? MembershipType { get; set; }

    public ICollection<Rental> Rentals { get; set; } = new List<Rental>();
}