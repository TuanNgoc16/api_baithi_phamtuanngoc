using System;
using Microsoft.EntityFrameworkCore;
namespace tuanngoc
 
{
    public class MemDb : DbContext
    {
        public MemDb(DbContextOptions<MemDb> options) : base(options){ }
        public DbSet<Mem> Mems => Set<Mem>();
    }
}
