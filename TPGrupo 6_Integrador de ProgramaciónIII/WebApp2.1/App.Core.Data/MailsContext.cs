using App.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Data
{
    public class MailsContext : DbContext
    {
        public MailsContext(DbContextOptions<MailsContext> options) : base(options)
        {
        }

        public DbSet<Mail> Mails { get; set; }
        public DbSet<UserTable> Users { get; set; } // Agrega esta línea para el DbSet de la entidad UserTable

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString =
                "Persist Security Info=True;Initial Catalog=Mails;Data Source=.; Integrated Security=True;TrustServerCertificate=True;";

            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}