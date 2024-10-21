using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	[SerializeField]
	private Button newGameBtn;

	[SerializeField]
	private Button continueBtn;

	[SerializeField]
	private Canvas messageBox;

	[SerializeField]
	private Button yesBtn;

	[SerializeField]
	private Button cancelBtn;

	private bool hasSaveFile;

	void Start()
	{
		hasSaveFile = DataPersistenceManager.Instance.HasSaveFile();

		if (!hasSaveFile)
			continueBtn.gameObject.SetActive(false);
	}

	public void OnNewGameClicked()
	{
		if (hasSaveFile)
			EnableWarningMessageBox();
		else
			StartNewGame();
	}

	public void OnContinueClicked()
	{
		DisableMainButtons();
		LevelManager.Instance.LoadScene();
	}

	public void OnYesClicked()
	{
		StartNewGame();
	}

	private void StartNewGame()
	{
		DisableMainButtons();
		DataPersistenceManager.Instance.NewGame();
		LevelManager.Instance.LoadScene(1);
	}

	public void OnCancelClicked()
	{
		messageBox.gameObject.SetActive(false);
		EnableMainButtons();
	}

	private void EnableWarningMessageBox()
	{
		messageBox.gameObject.SetActive(true);
		DisableMainButtons();
	}

	private void DisableMainButtons()
	{
		newGameBtn.interactable = false;
		continueBtn.interactable = false;
	}

	private void EnableMainButtons()
	{
		newGameBtn.interactable = true;
		continueBtn.interactable = true;
	}
}
