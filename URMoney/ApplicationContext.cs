using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace URMoney
{
    internal class ApplicationContext: DbContext
    {
        public DbSet<Operation> Operations { get; set; } = null!;
        public DbSet<Transaction> Transactions { get; set; } = null!;
        public DbSet<Type> Types { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Valute> Valutes { get; set; } = null!;
        public DbSet<People> Peoples { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=database.db");
        }
    }
}
