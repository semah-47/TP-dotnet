namespace ASPCoreFirstApp.ViewModels;

public class CustomerListItemVM
{
    public string Name { get; set; } = "";
    public string? Email { get; set; }
    public string MembershipName { get; set; } = "";
    public int DiscountPercent { get; set; }
}