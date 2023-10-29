using Microsoft.EntityFrameworkCore;
using Nexall.Data.Models;

namespace Nexall.Data.DataContext
{
    public class NexallContext : DbContext
    {
        public NexallContext(DbContextOptions<NexallContext> options)
            : base(options)
        {
        }

        public DbSet<Statistics> Statistics { get; set; }
    }
}