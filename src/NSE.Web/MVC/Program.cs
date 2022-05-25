using MVC.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentityConfiguration();

builder.Services.AddMvcConfiguration();

var app = builder.Build();

app.UseMvcConfiguration(builder.Environment);

app.Run();
