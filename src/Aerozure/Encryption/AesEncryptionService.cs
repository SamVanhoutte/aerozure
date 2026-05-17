using Microsoft.Extensions.Options;

namespace Aerozure.Encryption;

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;


public class AesEncryptionService(IOptions<EncryptionSettings> encryptionSettings) : IEncryptionService
{
    private readonly byte[] key = Encoding.UTF8.GetBytes(encryptionSettings.Value.SecretKey);
    private readonly byte[] initializationVector = Encoding.UTF8.GetBytes(encryptionSettings.Value.InitializationVector);

    public string Encrypt(string plaintext)
    {
        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = initializationVector;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using var encryptor = aes.CreateEncryptor();
        using var ms = new MemoryStream();
        using (var cryptoStream = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        using (var sw = new StreamWriter(cryptoStream))
        {
            sw.Write(plaintext);
        }

        return Convert.ToBase64String(ms.ToArray());
    }

    public string Decrypt(string ciphertext)
    {
        var buffer = Convert.FromBase64String(ciphertext);

        using var aes = Aes.Create();
        aes.Key = key;
        aes.IV = initializationVector;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        using var decryptor = aes.CreateDecryptor();
        using var ms = new MemoryStream(buffer);
        using var cryptoStream = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cryptoStream);
        return sr.ReadToEnd();
    }
}