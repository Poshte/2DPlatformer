using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour, IDataPersistence
{
	private static LevelManager _instance;
	public static LevelManager Instance
	{
		get
		{
			if (_instance == null)
			{
				Debug.LogError("LevelManager is null");
			}

			return _instance;
		}
	}

	private readonly float waitTime = 0.5f;

	//Forest is the default scene when starting a new game
	private int sceneIndex/* = 1*/;

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

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.V))
		{
			//TODO
			//replace this with end of scene trigger
			if (Input.GetKey(KeyCode.LeftShift))
				LoadPreviousScene();
			else
				LoadNextScene();
		}
	}

	private void LoadNextScene()
	{
		LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	private void LoadPreviousScene()
	{
		LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
	}

	private IEnumerator LoadSceneAsynchronously(int index)
	{
		//fire BeforeSceneDestroyed event
		GameEvents.Instance.BeforeSceneDestroyed();

		yield return new WaitForSeconds(waitTime);
		SceneManager.LoadSceneAsync(index);
	}

	public void LoadScene(int? index = null)
	{
		index ??= sceneIndex;
		StartCoroutine(LoadSceneAsynchronously(index.Value));
	}

	public void LoadData(GameData data)
	{
		sceneIndex = data.sceneIndex;
	}

	public void SaveData(GameData data)
	{
		data.sceneIndex = SceneManager.GetActiveScene().buildIndex;
	}
}
