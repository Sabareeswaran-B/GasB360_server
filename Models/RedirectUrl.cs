namespace GasB360_server.Models;

public class RedirectUrl<T>
{
    public string? Origin { get; set; }

    public T? data {get; set;}
}
