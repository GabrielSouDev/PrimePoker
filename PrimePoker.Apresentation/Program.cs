using PrimePoker.Application.Interfaces;
using PrimePoker.Application.Services;
using Redis.OM;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Serviços
builder.Services.AddSingleton<SessionService>();
builder.Services.AddSingleton<ISessionNotifier, SessionNotifier>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapHub<SessionHub>("/Session");
app.MapControllers();

app.Run();
