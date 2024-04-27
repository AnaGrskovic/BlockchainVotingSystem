using VotingApp.Contracts.Services;
using VotingApp.Services;
using VotingApp.Contracts.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<AzureStorageSettings>(builder.Configuration.GetSection(AzureStorageSettings.Section));

builder.Services.AddScoped<IHttpClientService, HttpClientService>();
builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
builder.Services.AddScoped<IMessageQueueService, AzureMessageQueueService>();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.Run();

