namespace Resolv.Infrastructure;

public class DatabaseOptions
{
    public static string Key = "Database:ResolvDb";
    public string ConnectionString { get; set; } = string.Empty;
}
