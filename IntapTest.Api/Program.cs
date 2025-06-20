using IntapTest.Api;

var builder = WebApplication.CreateBuilder(args);

var startup = new StartUp(builder.Environment);
await startup.ConfigureServices(builder.Services);

var app = builder.Build();
await startup.Configure(app, builder.Environment, builder.Services);
