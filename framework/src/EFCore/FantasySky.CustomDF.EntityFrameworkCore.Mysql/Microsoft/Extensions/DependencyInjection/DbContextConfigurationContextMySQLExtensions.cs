using FantasySky.CustomDF.Modularity;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class DbContextConfigurationContextMySQLExtensions
{
    public static DbContextOptionsBuilder UseMysqlDb(
        this DbContextOptionsBuilder builder,
        ServiceConfigurationContext context,
        string connectionName)
    {
        var connectionString = context.Configuration.GetConnectionString(connectionName);

        builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

        return builder;
    }
}
