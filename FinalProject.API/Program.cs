using FinalProject.BL.Extensions;
using FinalProject.DAL.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Konfigurasi CORS agar Flutter Web bisa akses API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFlutter",
        policy =>
        {
            policy
            .SetIsOriginAllowed(origin =>
                origin.StartsWith("http://localhost:") ||
                origin.StartsWith("http://192.168.1.") // ganti dengan IP lokal PC Anda jika berbeda
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
});

// Add services to the container.
builder.Services.AddBusinessLogicLayer(builder.Configuration);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null; // Use original property names
});

// Add Swagger with JWT authentication
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { 
        Title = "FinalProject API", 
        Version = "v1" 
    });

    // Configure JWT authentication for Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Aktifkan CORS sebelum authentication/authorization
app.UseCors("AllowFlutter");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
