using DummyAuthorizationProvider.API.Middleware;
using DummyAuthorizationProvider.Contracts.Services;
using DummyAuthorizationProvider.Contracts.UoW;
using DummyAuthorizationProvider.Data.Db.Context;
using DummyAuthorizationProvider.Data.Db.UoW;
using DummyAuthorizationProvider.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("VoterDb"),
        opt => opt.MigrationsAssembly("DummyAuthorizationProvider.Data.Db"));
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();


app.UseMiddleware<ExceptionHandlerMiddleware>();

app.MapControllers();

app.Run();
