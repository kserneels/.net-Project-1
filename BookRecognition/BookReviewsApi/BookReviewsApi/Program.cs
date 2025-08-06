using BookReviewsApi.Data;
using BookReviewsApi.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ? Voeg de databaseverbinding toe
builder.Services.AddDbContext<BookReviewDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ? Voeg repositories toe vóór builder.Build()
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookReviewRepository, BookReviewRepository>();

// ? Voeg controllers en Swagger toe
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;

    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ? Configureer CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

// ? Pas migraties toe bij startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<BookReviewDbContext>();
    dbContext.Database.Migrate();
}

// ? Middleware configuratie
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
