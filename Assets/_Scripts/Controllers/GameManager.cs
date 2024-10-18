using Assets._Scripts.BaseInfos;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private const float waitTime = 2f;

	private void Start()
	{
		GameObject.FindGameObjectWithTag(Constants.Tags.Player).GetComponent<Player>().enabled = true;

		GameEvents.Instance.OnBeforeSceneLoad += OnBeforeSceneLoad;
		GameEvents.Instance.OnAfterSceneLoad += OnAfterSceneLoad;
	}

	private void OnBeforeSceneLoad()
	{
		DisablePlayerScript();
	}

	private void OnAfterSceneLoad()
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
