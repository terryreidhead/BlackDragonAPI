using System;
using Microsoft.EntityFrameworkCore;

namespace BlackDragonAPI.Models
{
    public class TokenBlacklist
    {
        public string Token { get; set; } = string.Empty;
        public DateTime RevokedAt { get; set; }
    }

    public class TokenBlacklistContext : DbContext
    {
        public DbSet<TokenBlacklist> Tokens { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           // optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        }
       
    }
}
