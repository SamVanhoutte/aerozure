namespace Aerozure.Configuration
{
    public class KeyVaultSettings
    {
        public string Uri { get; set; } = null!;
        public string TokenSigningKey { get; set; }= null!;
    }
}