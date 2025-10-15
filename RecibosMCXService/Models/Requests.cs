using Microsoft.EntityFrameworkCore;
namespace RecibosMCXService.Models
{
    public class Requests
    {
        public int Id { get; set; }
        public string? BrowserAgent { get; set; }
        public string? IpAddress { get; set; }
        public DateTime Date { get; set; }
    }

    public class RecibosMCXDb : DbContext
    {
        public RecibosMCXDb(DbContextOptions options) : base(options) { }
        public DbSet<Requests> Requests { get; set; } = null!;
    }
}