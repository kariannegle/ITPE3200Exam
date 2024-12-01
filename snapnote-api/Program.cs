using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NoteApp.Data;
using NoteApp.Models;
using NoteApp.Repositories;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container
builder.Services.AddControllers(); // Enable controllers
builder.Services.AddEndpointsApiExplorer(); // Enable Swagger endpoint discovery
builder.Services.AddSwaggerGen(); // Enable Swagger generation
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add DbContext and Identity services
builder.Services.AddDbContext<NoteAppContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("NoteAppContext")));

// Identity configuration
builder.Services.AddIdentity<User, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddEntityFrameworkStores<NoteAppContext>()
    .AddDefaultTokenProviders();

// Add authentication middleware
builder.Services.AddAuthentication();

// Register the repositories
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IFriendRepository, FriendRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

Log.Information("Current Environment: {Environment}", app.Environment.EnvironmentName);

// Handle errors and HSTS for production
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Enable Swagger only in development (for security)

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "NoteApp API v1");
    c.RoutePrefix = string.Empty; // Makes Swagger the default page
});


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Apply CORS policy
app.UseCors("AllowAll");

// Use authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Map controllers
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Post}/{action=Index}/{id?}");

app.Run();
