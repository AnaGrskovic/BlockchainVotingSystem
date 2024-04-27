using VotingApp.Contracts.Services;
using VotingApp.Services;
using VotingApp.Contracts.Settings;
using VotingApp.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<AuthorizationSettings>(builder.Configuration.GetSection(AuthorizationSettings.Section));
builder.Services.Configure<AzureStorageSettings>(builder.Configuration.GetSection(AzureStorageSettings.Section));

builder.Services.AddHttpClient();
builder.Services.AddScoped<IHttpClientService, HttpClientService>();
builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
builder.Services.AddScoped<IMessageQueueService, AzureMessageQueueService>();
builder.Services.AddScoped<IVotingService, VotingService>();


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

