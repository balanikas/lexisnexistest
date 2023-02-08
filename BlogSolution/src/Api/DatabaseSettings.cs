namespace Api;

public class DatabaseSettings
{
    public const string SettingsName = "Database";

    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string AuthorsCollectionName { get; set; } = null!;
    public string BlogsCollectionName { get; set; } = null!;
}