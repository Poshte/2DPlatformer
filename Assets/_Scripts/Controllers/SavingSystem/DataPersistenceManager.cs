using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

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
			Destroy(gameObject);
		}
		else
		{
			_instance = this;
		}

		DontDestroyOnLoad(gameObject);
	}

	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
		SceneManager.sceneUnloaded += OnSceneUnloaded;
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
		SceneManager.sceneUnloaded -= OnSceneUnloaded;
	}

	private void Update()
	{
		//TODO
		//remove this in deployment
		if (Input.GetKeyDown(KeyCode.F1))
		{
			SaveGame();
		}
	}

	private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
	{
		dataPersistenceObjects = FindDataPersistenceObjects();
		LoadGame();
	}

	private void OnSceneUnloaded(Scene arg0)
	{
		SaveGame();
	}

	private void OnApplicationQuit()
	{
		SaveGame();
	}

	private void SaveGame()
	{
		if (gameData == null)
			return;

		foreach (IDataPersistence obj in dataPersistenceObjects)
		{
			obj.SaveData(gameData);
		}

		SavingSystem.Save(gameData);
	}

	private void LoadGame()
	{
		gameData = SavingSystem.Load();

		//if there isn't any data yet, initialize a new one
		if (gameData == null)
			return;

		foreach (IDataPersistence obj in dataPersistenceObjects)
		{
			obj.LoadData(gameData);
		}
	}

	private List<IDataPersistence> FindDataPersistenceObjects()
	{
		return FindObjectsOfType<MonoBehaviour>().
					OfType<IDataPersistence>().
					ToList();
	}

	public void NewGame()
	{
		gameData = new GameData();
	}

	public bool HasSaveFile()
	{
		return gameData != null;
	}
}
