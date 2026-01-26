using BlackDragonAPI.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

/// Create builder
var builder = WebApplication.CreateBuilder(args);

/// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

/// Configure DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

/// Configure Identity
builder.Services.AddIdentityCore<IdentityUser>(options =>
{
    options.Password.RequiredLength = 8;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

/// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");

/// Get the secret key
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

/// Configure Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

/// Configure Authorization 
builder.Services.AddAuthorization();


/// Add services to the container.

builder.Services.AddControllers();

/// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

/// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
/// app.UseRouting();
app.UseHttpsRedirection();

/// Configure Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

/// Run the application
app.Run();
