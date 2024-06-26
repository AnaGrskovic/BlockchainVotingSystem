using DummyAuthorizationProvider.API.Middleware;
using DummyAuthorizationProvider.Contracts.Services;
using DummyAuthorizationProvider.Contracts.Settings;
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

builder.Services.Configure<TimeSettings>(builder.Configuration.GetSection(TimeSettings.Section));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
builder.Services.AddScoped<ITimeService, TimeService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowOrigin");


app.UseMiddleware<ExceptionHandlerMiddleware>();

app.MapControllers();

app.Run();
