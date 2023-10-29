using Microsoft.EntityFrameworkCore;
using NEXALL.Models;

namespace NEXALL.DataContext
{
    public class NEXALLContext : DbContext
    {
        public NEXALLContext(DbContextOptions<NEXALLContext> options)
            : base(options)
        {
        }

        public DbSet<Statistics> Statistics { get; set; }
    }
}