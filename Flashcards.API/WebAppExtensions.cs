using System.Data;
using System.Reflection;
using Dapper;

namespace Flashcards.API;

public static class WebAppExtensions
{
    public static async Task InitDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var connection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
        var sql = await File.ReadAllTextAsync("db.sql");
        await connection.ExecuteAsync(sql);
    }

    public static void UseDapperTypeHandlers(this WebApplication app)
    {
        var typeHandlers = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(x =>
                x.BaseType is { IsGenericType: true } &&
                x.BaseType.GetGenericTypeDefinition() == typeof(SqlMapper.TypeHandler<>));
        foreach (var typeHandler in typeHandlers)
        {
            var type = typeHandler.BaseType?.GetGenericArguments().SingleOrDefault();
            if (type != null)
            {
            }
            SqlMapper.AddTypeHandler(type, (SqlMapper.ITypeHandler)Activator.CreateInstance(typeHandler)!);
        }
    }
}