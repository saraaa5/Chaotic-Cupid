using CupidonServer.Hubs;
using CupidonServer.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IPersonService, PersonService>();
builder.Services.AddSingleton<CupidonService>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<CupidonService>());
builder.Services.AddSingleton<ICupidonService>(provider => provider.GetRequiredService<CupidonService>());
builder.Services.AddSignalR();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapHub<CupidonHub>("/cupidonHub");

app.Run();