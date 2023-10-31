using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Nexall.Data.Models;

namespace Nexall.Data
{
    public interface INexallContext
    {
        DbSet<Statistics> Statistics { get; set; }

        int SaveChanges();
        EntityEntry Update(object entity);
    }
}