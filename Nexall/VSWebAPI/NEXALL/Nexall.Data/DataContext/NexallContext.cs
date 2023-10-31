using Microsoft.EntityFrameworkCore;
using Nexall.Data.Models;

namespace Nexall.Data.DataContext
{
    public class NexallContext : DbContext, INexallContext

    {
        public NexallContext(DbContextOptions<NexallContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Statistics> Statistics { get; set; }
    }
}