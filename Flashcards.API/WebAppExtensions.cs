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
        var typeHandlers = app.Services.GetServices<SqlMapper.ITypeHandler>();
        foreach (var typeHandler in typeHandlers)
        {
            SqlMapper.AddTypeHandler(typeHandler.GetType(), typeHandler);
        }
    }
}