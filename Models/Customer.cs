namespace SaaS.LicenseManager.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public required string FullName { get; set; } 
        public required string Country { get; set; } 
        public required string PhoneNumber { get; set; } 
        public required string EmailAddress { get; set; } 

        public string LicenseKey { get; set; } = string.Empty;
        public DateTime LicenseStart { get; set; }
        public DateTime LicenseEnd { get; set; }
        public bool IsActive { get; set; }
        public LicenseType LicenseType { get; set; }
    }

    public enum LicenseType
    {
        Trial7Days,
        Monthly,
        Yearly
    }
}