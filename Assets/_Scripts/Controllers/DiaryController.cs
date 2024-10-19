using Assets._Scripts.BaseInfos;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DiaryController : MonoBehaviour, IDataPersistence
{
	//diary elements
	[SerializeField]
	private GameObject cover;
	private Image leftPage;
	private Image rightPage;

	//left & right pages index
	private int leftIndex = 0;
	private int rightIndex = 1;

	//maximum pages control
	private const int maxIndex = 9;

	//pages
	[SerializeField]
	private Sprite[] photos;

	[SerializeField]
	private Sprite[] diaryPages;

	private int currentPageCount;

	//dependencies
	private Player playerScript;
	private Controller2D controller2D;

	private void Awake()
	{
		playerScript = GameObject.FindGameObjectWithTag(Constants.Tags.Player).GetComponent<Player>();
		controller2D = GameObject.FindGameObjectWithTag(Constants.Tags.Player).GetComponent<Controller2D>();

		var temp = cover.GetComponentsInChildren<Image>();
		leftPage = temp[1];
		rightPage = temp[2];
	}

	void Update()
	{
		if (Keyboard.current.tabKey.wasPressedThisFrame && controller2D.info.bottomCollision)
		{
			if (!cover.activeSelf)
			{
				cover.SetActive(true);
				playerScript.enabled = false;
				DisplayDiary();
			}
			else
			{
				cover.SetActive(false);
				playerScript.enabled = true;
				playerScript.GetMovementInput(Vector2.zero);
			}
		}

		if (cover.activeSelf)
		{
			if (Keyboard.current.aKey.wasPressedThisFrame)
				TurnPage(-2);
			else if (Keyboard.current.dKey.wasPressedThisFrame)
				TurnPage(2);
		}
	}

	private void TurnPage(int v)
	{
		leftIndex += v;
		rightIndex += v;

		//checking first and last page indexes
		if (leftIndex < 0)
		{
			leftIndex = 0;
			rightIndex = 1;
			return;
		}
		else if (rightIndex > maxIndex)
		{
			leftIndex = maxIndex - 1;
			rightIndex = maxIndex;
			return;
		}

		DisplayDiary();
	}

	private void DisplayDiary()
	{
		if (leftIndex >= 0 && rightIndex <= maxIndex)
		{
			leftPage.overrideSprite = diaryPages[leftIndex];
			rightPage.overrideSprite = diaryPages[rightIndex];
		}
	}
		
	public void AddPhotosToDiary(int[] pageIndexes)
	{
		foreach (var index in pageIndexes)
		{
			diaryPages[index] = photos[index];
			currentPageCount++;
		}
	}

	public void PlayPageAddedToDiarySound()
	{
		//TODO
		//might wanna change this.transform.position to player.transform.position
		AudioManager.Instance.PlayOneShot(FMODEvents.Instance.DiaryAddedSound, this.transform.position);
	}

	public void ShowPageAddedToDiaryVisual()
	{
		//TODO
		//show visuals
	}

	public void LoadData(GameData data)
	{
		LoadDiaryPages(data.activeDiaryPages);
	}

	public void SaveData(GameData data)
	{
		data.activeDiaryPages = currentPageCount;
	}

	private void LoadDiaryPages(int loadedIndex)
	{
		var loadedArray = Enumerable.Range(0, loadedIndex).ToArray();
		AddPhotosToDiary(loadedArray);
	}
}