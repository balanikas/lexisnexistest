using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using MongoDB.Driver;

namespace Api.IntegrationTests;

public class Factory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    private readonly string _connString = "mongodb://root:rootpassword@localhost:27017";
    private readonly string _databaseName;

    public Factory()
    {
        _databaseName = $"test_db_{Guid.NewGuid()}";
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.Configure<DatabaseSettings>(opts =>
            {
                opts.DatabaseName = _databaseName;
                opts.ConnectionString = _connString;
            });
        });


        builder.UseEnvironment("Development");
    }

    protected override void Dispose(bool disposing)
    {
        var client = new MongoClient(_connString);
        client.DropDatabase(_databaseName);
        base.Dispose(disposing);
    }
}