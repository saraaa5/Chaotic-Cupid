using CupidonServer.Hubs;
using CupidonServer.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IPersonService, PersonService>();
builder.Services.AddHostedService<CupidonService>();
builder.Services.AddSingleton<ICupidonService>(provider =>
    (ICupidonService)provider.GetRequiredService<IHostedService>());
builder.Services.AddSignalR();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapHub<CupidonHub>("/cupidonHub");

app.Run();