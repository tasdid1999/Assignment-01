using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentCRUD.DataAccess;
using StudentCRUD.Model.Helper;
using StudentCRUD.Repository;
using StudentCRUD.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("conn")));
builder .Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();

//automapper configuration
var automapper = new MapperConfiguration(item => item.AddProfile(new MapperHandler()));
IMapper mapper = automapper.CreateMapper();
builder.Services.AddSingleton(mapper);

//database configuration for Dapper
builder.Services.AddSingleton(serviceProvider =>
{
    var configuration = serviceProvider.GetService<IConfiguration>();

    var connectionString = configuration.GetConnectionString("conn") ?? throw new Exception("connection string not found");

    return new SqlConnectionFactory(connectionString);
}

);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
