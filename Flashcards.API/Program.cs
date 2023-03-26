using FastEndpoints;
using FastEndpoints.Swagger;
using Flashcards.API;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints(options =>
{
    options.SourceGeneratorDiscoveredTypes = DiscoveredTypes.All;
});

builder.Services.AddMediator(o =>
{
    o.ServiceLifetime = ServiceLifetime.Scoped;
});

builder.Services.AddDbConnection(builder.Configuration["Database:ConnectionString"]);

builder.Services.AddSwaggerDoc();


var app = builder.Build();

await app.InitDb();

app.UseDapperTypeHandlers();

app.UseDefaultExceptionHandler();
app.UseAuthorization();
app.UseFastEndpoints();
app.Map("/", () => Results.Redirect("/swagger"));
app.UseSwaggerGen(uiConfig: settings => settings.ConfigureDefaults());

app.Run();
