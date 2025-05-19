using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Service_Image.api.Extensions;
using Service_Image.api.Infrastructure.Core.Data;
using Service_Image.Api.Domaine.Core.DTO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ImageDbContext>();
//configuration de Dbcontext dans program.cs 
//builder.Service:c'est pour utiliser Injection de dependance
builder.Services.AddDbContext<ImageDbContext>(options =>
//UseSqlServer: provider de SQl server
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//Active le system de l'authentification de JWT
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
