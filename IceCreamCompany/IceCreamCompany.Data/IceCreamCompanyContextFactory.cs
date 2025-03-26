using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace IceCreamCompany.Data
{
    public class IceCreamCompanyContextFactory : IDesignTimeDbContextFactory<IceCreamCompanyContext>
    {
        public IceCreamCompanyContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<IceCreamCompanyContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new IceCreamCompanyContext(optionsBuilder.Options);
        }
    }
}
