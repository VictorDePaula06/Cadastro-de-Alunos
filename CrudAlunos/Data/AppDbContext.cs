using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrudAlunos.Models;
using Microsoft.EntityFrameworkCore;

namespace CrudAlunos.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<ModeloContato> Contatos { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost;Database=MeusContatos;Trusted_Connection=True;TrustServerCertificate=True");
            }
        }

    }
}