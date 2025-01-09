namespace Aerozure.Communication.Context;

public class EmailAddress
{
    public EmailAddress(string address, string? name)
    {
        Name = name ?? address;
        Address = address;
    }
    public string? Name { get; set; }
    public string Address { get; set; }
}