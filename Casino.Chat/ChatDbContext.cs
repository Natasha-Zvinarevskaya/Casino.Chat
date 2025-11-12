using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Casino.Chat.DataContext
{
    public class ChatDbContext:DbContext
    {
         public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options)
    {

    }
    
        public DbSet<Letter>Letters { get; set; }
   
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Letter>().HasKey(u => u.Id);

        }

    }
}
