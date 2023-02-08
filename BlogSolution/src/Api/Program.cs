using Api;
using Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection(DatabaseSettings.SettingsName));
builder.Services.AddSingleton<IDatabaseContext, DatabaseContext>();
builder.Services.AddSingleton<IAuthorsRepository, AuthorsRepository>();
builder.Services.AddSingleton<IBlogsRepository, BlogsRepository>();
builder.Services.AddSingleton<ITransactionHandler, TransactionHandler>();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseExceptionHandler();
app.UseStatusCodePages();
if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage();

//app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

namespace Api
{
    public class Program
    {
    }
}


/*

- todo: use automapper or equivalent package
- todo: usage metrics and performance metrics, integrate third party opentelemetry impl
- todo: use mongodb transactions
- todo: use structured logging such as serilog
- todo: unit tests for controllers/services
- todo: extract businesslogic layer and database layer to separate project
- todo: complete crud operations for the entities, currntly missing PUT,DELETE,etc
*/