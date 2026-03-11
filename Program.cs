using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentManagementAPI.Data;
using StudentManagementAPI.Repositories;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// -------------------------
// CORS Policy
// -------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

// -------------------------
// Controllers
// -------------------------
builder.Services.AddControllers();

// -------------------------
// Database
// -------------------------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// -------------------------
// Swagger
// -------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// -------------------------
// Repositories
// -------------------------
builder.Services.AddScoped<IStudentRepository, StudentRepository>();

// -------------------------
// JWT Authentication
// -------------------------
var key = Encoding.ASCII.GetBytes("YourSuperSecretKey123!"); // Replace with a secure key
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Bearer";
    options.DefaultChallengeScheme = "Bearer";
})
.AddJwtBearer("Bearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,        // Set true if you have an issuer
        ValidateAudience = false,      // Set true if you have an audience
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// -------------------------
// Build App
// -------------------------
var app = builder.Build();

// -------------------------
// Middleware
// -------------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();   // ← Must come BEFORE UseAuthorization
app.UseAuthorization();

app.MapControllers();

app.Run();