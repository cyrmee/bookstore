using Presentation.Configuration;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

ApplicationConfiguration.Configure(builder);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.MapScalarApiReference();
	app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();