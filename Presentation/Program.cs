using Presentation.Configuration;
using Presentation.Middlewares;

var builder = WebApplication.CreateBuilder(args);

ApplicationConfiguration.Configure(builder);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<AuthenticationMiddleware>();

app.MapControllers();
app.UseCors();

app.Run();