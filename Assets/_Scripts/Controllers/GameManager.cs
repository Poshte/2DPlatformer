using Assets._Scripts.BaseInfos;
using FMODUnity;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	private const float waitTime = 2f;
	private EventReference ambienceEventReference;

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

	private void DisablePlayerScript()
	{
		GameObject.FindGameObjectWithTag(Constants.Tags.Player).GetComponent<Player>().enabled = false;
	}

	private IEnumerator EnablePlayerAfterSceneIsLoaded()
	{
		yield return new WaitForSeconds(waitTime);

		GameObject.FindGameObjectWithTag(Constants.Tags.Player).GetComponent<Player>().enabled = true;
	}
}
