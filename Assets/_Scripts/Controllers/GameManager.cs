using Assets._Scripts.BaseInfos;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	private const float waitTime = 2f;

	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
		SceneManager.sceneUnloaded += OnSceneUnloaded;
	}

	private void Start()
	{
		GameObject.FindGameObjectWithTag(Constants.Tags.Player).GetComponent<Player>().enabled = true;
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
		SceneManager.sceneUnloaded -= OnSceneUnloaded;
	}

	private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
	{
		DisablePlayerScript();
	}

	private void OnSceneUnloaded(Scene arg0)
	{
		StartCoroutine(EnablePlayerAfterSceneIsLoaded());
	}

	private IEnumerator EnablePlayerAfterSceneIsLoaded()
	{
		yield return new WaitForSeconds(waitTime);
		EnablePlayerScript();
	}

	private void DisablePlayerScript()
	{
		GameObject.FindGameObjectWithTag(Constants.Tags.Player).GetComponent<Player>().enabled = false;
	}

	private void EnablePlayerScript()
	{
		GameObject.FindGameObjectWithTag(Constants.Tags.Player).GetComponent<Player>().enabled = true;
	}
}
