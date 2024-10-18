using System;
using System.IO;
using System.Security.Cryptography;

public class EncryptionUtility
{
	private static readonly byte[] key = Convert.FromBase64String("bR7r54wsR2VjiMwSh4XOHBPdTjtmcKCquiq/qopbJIY=");
	private static readonly byte[] iv = Convert.FromBase64String("u4DrCDLTksss+yg7Ps9ENg==");

	public static string Encrypt(string data)
	{
		using (var aes = Aes.Create())
		{
			aes.Key = key;
			aes.IV = iv;

			using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
			{
				using (var stream = new MemoryStream())
				{
					using (var crypteStream = new CryptoStream(stream, encryptor, CryptoStreamMode.Write))
					{
						using (var writer = new StreamWriter(crypteStream))
						{
							writer.Write(data);
						}
					}

					return Convert.ToBase64String(stream.ToArray());
				}
			}
		}
	}

	public static string Decrypt(string data)
	{
		using (var aes = Aes.Create())
		{
			aes.Key = key;
			aes.IV = iv;

			using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
			{
				using (var stream = new MemoryStream(Convert.FromBase64String(data)))
				{
					using (var crypteStream = new CryptoStream(stream, decryptor, CryptoStreamMode.Read))
					{
						using (var reader = new StreamReader(crypteStream))
						{
							return reader.ReadToEnd();
						}
					}
				}
			}
		}
	}
}
