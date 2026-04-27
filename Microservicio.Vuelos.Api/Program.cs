using Microservicio.Vuelos.Api.Extensions;
using Microservicio.Vuelos.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);


// Controllers
builder.Services.AddControllers();

// Versioning
builder.Services.AddApiVersioningDocumentation();

// JWT Authentication
builder.Services.AddJwtAuthentication(builder.Configuration);

// CORS
builder.Services.AddCorsPolicy(builder.Configuration);

// Swagger
builder.Services.AddSwaggerDocumentation();

// Project services (DbContext + DataManagement + Business)
builder.Services.AddProjectServices(builder.Configuration);

// Authorization
builder.Services.AddAuthorization();

var app = builder.Build();
app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});

// Swagger
app.UseSwaggerDocumentation();

// HTTPS
// En desarrollo evitamos redirección automática para no romper clientes
// que aún usan la URL http local (IIS Express).
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// CORS
app.UseCorsPolicy();

// Authentication / Authorization
app.UseAuthentication();
app.UseAuthorization();

// Global exception handling
app.UseMiddleware<ExceptionHandlingMiddleware>();

// Controllers
app.MapControllers();

app.Run();