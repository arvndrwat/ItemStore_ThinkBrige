using Microsoft.EntityFrameworkCore;
using ShopBridge.Api.Models;

namespace ShopBridge.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options ) : base(options)
        {

        }
        public DbSet<Item> Items { get; set; }
    }
}
