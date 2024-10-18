using System;
using System.IO;
using UnityEngine;

public static class SavingSystem
{
	public static void Save(GameData data)
	{
		var path = Path.Combine(Application.persistentDataPath, "gameSave.dat");

		try
		{
			var json = JsonUtility.ToJson(data);
			var encryptedData = EncryptionUtility.Encrypt(json);

			using (var stream = new FileStream(path, FileMode.Create))
			{
				using (var writer = new StreamWriter(stream))
				{
					writer.Write(encryptedData);
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("An error occured during saving data at: " + path + Environment.NewLine + " ErrorMessage: " + ex.Message);
		}
	}

	public static GameData Load()
	{
		var path = Path.Combine(Application.persistentDataPath, "gameSave.dat");
		GameData data = null;

		if (File.Exists(path))
		{
			try
			{
				using (var stream = new FileStream(path, FileMode.Open))
				{
					using (var reader = new StreamReader(stream))
					{
						var encryptedData = reader.ReadToEnd();
						var json = EncryptionUtility.Decrypt(encryptedData);
						data = JsonUtility.FromJson<GameData>(json);
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("An error occured during loading data at: " + path + Environment.NewLine + " ErrorMessage: " + ex.Message);
			}
		}

		return data;
	}
}
