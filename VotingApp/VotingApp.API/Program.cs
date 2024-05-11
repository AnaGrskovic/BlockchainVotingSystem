using VotingApp.Contracts.Services;
using VotingApp.Services;
using VotingApp.Contracts.Settings;
using VotingApp.API.Middleware;
using VotingApp.Data.Db.Context;
using Microsoft.EntityFrameworkCore;
using VotingApp.Contracts.UoW;
using VotingApp.Data.Db.UoW;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("VoteBackupDb"),
        opt => opt.MigrationsAssembly("VotingApp.Data.Db"));
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<AuthorizationSettings>(builder.Configuration.GetSection(AuthorizationSettings.Section));
builder.Services.Configure<CandidatesSettings>(builder.Configuration.GetSection(CandidatesSettings.Section));
builder.Services.Configure<AzureStorageSettings>(builder.Configuration.GetSection(AzureStorageSettings.Section));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IVotingService, VotingService>();
builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
builder.Services.AddScoped<IHttpClientService, HttpClientService>();
builder.Services.AddScoped<ICandidateService, CandidateService>();
builder.Services.AddScoped<IBackupService, BackupService>();
builder.Services.AddScoped<IMessageQueueService, AzureMessageQueueService>();


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

