using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
	[SerializeField]
	private Button resumeBtn;

	[SerializeField]
	private Button controlsBtn;

	[SerializeField]
	private Button quitBtn;

	[SerializeField]
	private Canvas pauseScreen;

	[SerializeField]
	private Image controlsImage;

	[SerializeField]
	private Button backBtn;

	private bool Unpaused;

	private void Start()
	{
		GameEvents.Instance.OnPauseButtonClicked += PauseButtonClicked;
	}
	
	private void OnApplicationQuit()
	{
		GameEvents.Instance.OnPauseButtonClicked -= PauseButtonClicked;
	}

	private void PauseButtonClicked()
	{
		if (Unpaused)
		{
			pauseScreen.gameObject.SetActive(false);
			Unpaused = false;
		}
		else
		{
			pauseScreen.gameObject.SetActive(true);
			Unpaused = true;
		}
	}

	public void OnResumeClicked()
	{
		pauseScreen.gameObject.SetActive(false);
		Unpaused = false;

		GameEvents.Instance.ResumeButtonClicked();
	}

	public void OnControlsClicked()
	{
		controlsImage.gameObject.SetActive(true);
		backBtn.gameObject.SetActive(true);
	}

	public void OnBackClicked()
	{
		controlsImage.gameObject.SetActive(false);
		backBtn.gameObject.SetActive(false);
	}

	public void OnQuitClicked()
	{
		if (Application.isEditor)
			EditorApplication.isPlaying = false;
		else
			Application.Quit();
	}
}
