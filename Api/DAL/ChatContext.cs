using Api.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.DAL
{
    public class ChatContext : DbContext
    {
        DbSet<Message> Messages { get; set; }
        public ChatContext(DbContextOptions options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>()
                .Property(f => f.Id)
                .ValueGeneratedOnAdd();
        }

    }

}
