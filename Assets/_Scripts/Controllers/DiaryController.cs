using Assets._Scripts.BaseInfos;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DiaryController : MonoBehaviour
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
	[SerializeField]
	private int maxIndex = 1;

	[SerializeField]
	private Sprite[] pages;

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
			leftPage.overrideSprite = pages[leftIndex];
			rightPage.overrideSprite = pages[rightIndex];
		}
	}

	//unlocking new entries in diary
	public void IncreamentDiaryIndex()
	{
		maxIndex += 2;
	}
}
