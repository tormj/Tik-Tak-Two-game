using DAL;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Load configuration for database and other settings
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")?
    .Replace("<%BasePath%>", FileHelper.BasePath) ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Register services
builder.Services.AddRazorPages()
    .AddSessionStateTempDataProvider();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.Parse(builder.Configuration["Session:IdleTimeout"] ?? "00:30:00");
    options.Cookie.Name = builder.Configuration["Session:CookieName"] ?? ".AspNetCore.Session";
});


// Register database context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

// Register repositories
// Use database-based repositories (default)
builder.Services.AddScoped<IConfigRepository, ConfigRepositoryDb>();
builder.Services.AddScoped<IGameRepository, GameRepositoryDb>();

// Uncomment the following lines to use JSON-based repositories instead
//builder.Services.AddScoped<IConfigRepository, ConfigRepositoryJson>();
//builder.Services.AddScoped<IGameRepository, GameRepositoryJson>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Detailed errors in development
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // Enable session state
app.UseAuthorization();

app.MapRazorPages();

app.Run();