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
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // Only allow the React frontend
              .AllowAnyMethod()                    // Allow all HTTP methods
              .AllowAnyHeader()                    // Allow any headers
              .AllowCredentials();                 // Allow credentials (cookies, authentication tokens)
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

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

// Other service configurations
builder.Services.AddScoped<IFriendRepository, FriendRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddControllers();

// Build the app once
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "NoteApp API v1");
        c.RoutePrefix = string.Empty;
    });
    app.UseHsts();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Apply CORS policy before authentication and authorization
app.UseCors("AllowReactApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();