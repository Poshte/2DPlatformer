using System;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
	private static GameEvents _instance;
	public static GameEvents Instance
	{
		get
		{
			return _instance;
		}
	}

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

	public event Action OnBeforeSceneDestroyed;
	public void BeforeSceneDestroyed() => OnBeforeSceneDestroyed?.Invoke();


	public event Action OnJumpButtonPressed;
	public void JumpButtonPressed() => OnJumpButtonPressed?.Invoke();


	public event Action OnConversationEnded;
	public void ConversationEnded() => OnConversationEnded?.Invoke();


	public event Action OnPauseButtonClicked;
	public void PauseButtonClicked() => OnPauseButtonClicked?.Invoke();


	public event Action OnResumeButtonClicked;
	public void ResumeButtonClicked() => OnResumeButtonClicked?.Invoke();
}