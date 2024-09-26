using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expense_Tracker_Data.Context
{
    public class DbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            string basePath = Path.Combine(Directory.GetCurrentDirectory(), @"..\Expense Tracker API\");


            var configuration = new ConfigurationBuilder().
                                    SetBasePath(basePath).
                                    AddJsonFile("appsettings.json", true).
                                    Build();

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            optionsBuilder.UseSqlServer(configuration.GetConnectionString("default"));

            return new ApplicationDbContext(optionsBuilder.Options);

        }
    }
}
