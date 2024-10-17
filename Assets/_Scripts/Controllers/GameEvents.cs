using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
	private static GameEvents _instance;
	public static GameEvents Instance
	{
		get
		{
			if (_instance == null)
			{
				Debug.LogError("GameEvents is null");
			}

			return _instance;
		}
	}

	private void Awake()
	{
		if (_instance != null)
		{
			Debug.LogError("More than one instance of GameEvents exist");
		}
		else
		{
			_instance = this;
		}
	}

	public event Action OnBeforeSceneLoad;
	public void BeforeSceneLoad() => OnBeforeSceneLoad?.Invoke();


	public event Action OnAfterSceneLoad;
	public void AfterSceneLoad() => OnAfterSceneLoad?.Invoke();


	public event Action OnConversationEnded;
	public void ConversationEnded() => OnConversationEnded?.Invoke();
}
