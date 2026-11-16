using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASPCoreFirstApp.Models;
using ASPCoreFirstApp.ViewModels;

namespace ASPCoreFirstApp.Controllers;

public class CustomerController : Controller
{
    private readonly RentalDbContext _db;

    public CustomerController(RentalDbContext db)
    {
        _db = db;
    }
    
    public async Task<IActionResult> Index()
    {
        var queryInclude = _db.Customers
            .AsNoTracking()
            .Include(c => c.MembershipType)
            .OrderBy(c => c.Name);

        Console.WriteLine("=== SQL INCLUDE ===");
        Console.WriteLine(queryInclude.ToQueryString());
        
        var querySelect = _db.Customers
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .Select(c => new CustomerListItemVM
            {
                Name = c.Name,
                Email = c.Email,
                MembershipName = c.MembershipType!.Name,
                DiscountPercent = c.MembershipType.DiscountPercent
            });

        Console.WriteLine("=== SQL SELECT ===");
        Console.WriteLine(querySelect.ToQueryString());

        var customers = await querySelect.ToListAsync();
        return View(customers);
    }
    
    public IActionResult Create()
    {
        LoadMemberships();
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Customer c)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            LoadMemberships();
            return View(c);
        }

        _db.Customers.Add(c);
        _db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    private void LoadMemberships()
    {
        ViewBag.Memberships = _db.MembershipTypes
            .AsNoTracking()
            .Select(m => new SelectListItem { Text = m.Name, Value = m.Id.ToString() })
            .ToList();
    }
}