namespace Aerozure.Communication.Context;

public class EmailAddress(string address, string? name)
{
    public string? Name { get; set; } = name ?? address;
    public string Address { get; set; } = address;
}