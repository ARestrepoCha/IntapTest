using IntapTest.Api;
using IntapTest.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var startup = new StartUp(builder.Environment);
startup.ConfigureServices(builder.Services);


// Add services to the container.
//builder.Services.AddDbContext<IntapTestDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

////Add DI for data services
//var data = new StartupDataConfiguration();
//data.DataConfigureServices(builder.Services);

// Configura Identity para usar el DbContext
//builder.Services.AddIdentity<IdentityUser, IdentityRole>()
//    .AddEntityFrameworkStores<IntapTestDbContext>()
//    .AddDefaultTokenProviders();

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var app = builder.Build();
await startup.Configure(app, builder.Environment, builder.Services);

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

//app.Run();
