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
builder.Services.Configure<PeersSettings>(builder.Configuration.GetSection(PeersSettings.Section));
builder.Services.Configure<TimeSettings>(builder.Configuration.GetSection(TimeSettings.Section));
builder.Services.Configure<ThresholdsSettings>(builder.Configuration.GetSection(ThresholdsSettings.Section));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IVotingService, VotingService>();
builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
builder.Services.AddScoped<IHttpClientService, HttpClientService>();
builder.Services.AddScoped<ICandidateService, CandidateService>();
builder.Services.AddScoped<IBackupService, BackupService>();
builder.Services.AddScoped<IMessageQueueService, AzureMessageQueueService>();
builder.Services.AddScoped<IDigitalSignatureService, DigitalSignatureService>();
builder.Services.AddScoped<ISecureBlockChainService, SecureBlockChainService>();
builder.Services.AddScoped<IBlockChainService, BlockChainService>();
builder.Services.AddScoped<IBlockChainResultService, BlockChainResultService>();
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

