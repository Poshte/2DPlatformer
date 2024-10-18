using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
	private static DataPersistenceManager _instance;
	public static DataPersistenceManager Instance
	{
		get
		{
			if (_instance == null)
			{
				Debug.LogError("DataPersistenceManager is null");
			}

			return _instance;
		}
	}

	private GameData gameData;

	private List<IDataPersistence> dataPersistenceObjects = new();

	private void Awake()
	{
		if (_instance != null)
		{
			Debug.LogError("More than one instance of DataPersistenceManager exist");
		}
		else
		{
			_instance = this;
		}
	}

	private void Start()
	{
		dataPersistenceObjects = FindDataPersistenceObjects();


		LoadGame();
	}

	private float savingTime = 3f;
	private float timer;

	private void Update()
	{
		if (timer >= savingTime)
		{
			SaveGame();
		}
		else
		{
			timer += Time.deltaTime;
		}
	}

	public void SaveGame()
	{
		foreach (IDataPersistence obj in dataPersistenceObjects)
		{
			obj.SaveData(gameData);
		}

		SavingSystem.Save(gameData);
	}

	public void LoadGame()
	{
		gameData = SavingSystem.Load();

		//if there isn't any data yet, initialize a new one
		if (gameData == null)
		{
			gameData = NewGame();
		}

		foreach (IDataPersistence obj in dataPersistenceObjects)
		{
			obj.LoadData(gameData);
		}
	}

	private GameData NewGame()
	{
		return new GameData();
	}

	private List<IDataPersistence> FindDataPersistenceObjects()
	{
		return FindObjectsOfType<MonoBehaviour>().
					OfType<IDataPersistence>().
					ToList();
	}
}
