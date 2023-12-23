using Carter;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NoteProject.API.Database;
using NoteProject.API.Entities;
using NoteProject.API.Features.Courses.Commands;
using NoteProject.API.Features.Notes;
using NoteProject.API.Features.Notes.Queries;
using NoteProject.API.Shared;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"));
});

builder.Services.AddIdentity<AppUser, AppRole>(options =>
{ 
    options.User.RequireUniqueEmail = false; 
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false; 
    options.Password.RequireNonAlphanumeric = false; 
    options.Password.RequiredLength = 3;
}).AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = Configuration["Jwt:Issuer"],
            ValidAudience = Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SecretKey"]))
        };
    });

builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("https://localhost:7283") 
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

builder.Services.AddAuthorization();
var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblies(assembly));

builder.Services.AddCarter();
builder.Services.AddScoped<DownloadNoteEndpoint>();
builder.Services.AddValidatorsFromAssembly(assembly);
builder.Services.AddSingleton<IFileService, FileService>();
builder.Services.AddScoped<NoteService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowSpecificOrigin");
app.MapCarter();

app.Run();