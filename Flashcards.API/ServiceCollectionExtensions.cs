using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;

namespace Flashcards.API;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDbConnection(this IServiceCollection collection, string? connectionString)
    {
        collection.AddScoped<IDbConnection>(_ =>
        {
            var connection = SqliteFactory.Instance.CreateConnection();
            connection.ConnectionString = connectionString;
            return connection;
        });
        return collection;
    }

    public static IServiceCollection AddDapperTypeHandlers(this IServiceCollection collection)
    {
        collection.Scan(scan => scan
            .FromAssemblyOf<Program>()
            .AddClasses(classes => classes.AssignableTo(typeof(SqlMapper.TypeHandler<>)))
            .As<SqlMapper.ITypeHandler>()
            .WithTransientLifetime()
        );
        return collection;
    }
}