using MinimalAPI.ApiEndpoints;
using MinimalAPI.ApiServicesExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddSwagger();
builder.AddPersistence();
builder.Services.AddCors();
builder.AddAutenticationJwt();

var app = builder.Build();

app.MapAutenticationEndpoints();
app.MapCategoriesEndpoints();
app.MapProductsEndpoints();

var environment = app.Environment;

app.UseExceptionHandling(environment)
    .UseSwaggerEndpoints()
    .UseAppCors();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
public partial class Program { }