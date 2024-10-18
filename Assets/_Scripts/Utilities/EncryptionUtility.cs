using System;
using System.IO;
using System.Security.Cryptography;

public class EncryptionUtility
{
	private static readonly byte[] key = Convert.FromBase64String("bR7r54wsR2VjiMwSh4XOHBPdTjtmcKCquiq/qopbJIY=");
	private static readonly byte[] iv = Convert.FromBase64String("u4DrCDLTksss+yg7Ps9ENg==");

	public static byte[] Encrypt(byte[] data)
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
						crypteStream.Write(data, 0, data.Length);
					}

					return stream.ToArray();
				}
			}
		}
	}

	public static byte[] Decrypt(byte[] data)
	{
		using (var aes = Aes.Create())
		{
			aes.Key = key;
			aes.IV = iv;

			using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
			{
				using (var stream = new MemoryStream(data))
				{
					using (var crypteStream = new CryptoStream(stream, decryptor, CryptoStreamMode.Read))
					{
						using (var result = new MemoryStream())
						{
							crypteStream.CopyTo(result);
							return result.ToArray();
						}
					}
				}
			}
		}
	}
}
