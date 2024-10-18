using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SavingSystem
{
	public static void Save(GameData gameData)
	{
		var path = Application.persistentDataPath + "/ISOHV.dat";
		var formatter = new BinaryFormatter();

		using (var stream = new FileStream(path, FileMode.Create))
		{
			formatter.Serialize(stream, gameData);
		}

		var data = File.ReadAllBytes(path);
		var encryptedData = EncryptionUtility.Encrypt(data);
		File.WriteAllBytes(path, encryptedData);
	}

	public static GameData Load()
	{
		var path = Application.persistentDataPath + "/ISOHV.dat";

		if (File.Exists(path))
		{
			var encryptedData = File.ReadAllBytes(path);
			var data = EncryptionUtility.Decrypt(encryptedData);

			using (var stream = new MemoryStream(data))
			{
				var formatter = new BinaryFormatter();

				return formatter.Deserialize(stream) as GameData;
			}
		}

		return null;
	}
}
