using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalutemCRM.Database;

public partial class DatabaseContext
{
    public static DbContextOptionsBuilder<DatabaseContext> optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
    public static DbContextOptions<DatabaseContext>? options;
    private static bool _inited = false;

    public static DbContextOptions<DatabaseContext> ConnectionInit()
    {
        if (_inited)
            return options!;

        var builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory());
        builder.AddJsonFile("appsettings.json");

        var config = builder.Build();
        var connectionString = ""
            + $"Server={config.GetConnectionString("Server")};"
            + $"Initial Catalog={config.GetConnectionString("Catalog")};"
            + $"User Id={config.GetConnectionString("User")};"
            + $"Password={config.GetConnectionString("Password")};"
            + $"TrustServerCertificate=true";

        return options = optionsBuilder.UseSqlServer(connectionString).Options;
    }

    public static void ReCreateDatabase(bool run) => run.DoIf(x => ReCreateDatabase(), x => x);

    public static void ReCreateDatabase()
    {
        using (DatabaseContext db = new(DatabaseContext.ConnectionInit()))
            db.DatabaseInit();
    }
}
