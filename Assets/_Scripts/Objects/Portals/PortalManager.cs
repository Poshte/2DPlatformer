using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{
	//portal objects
	[SerializeField] private GameObject bluePortal;
	[SerializeField] private GameObject orangePortal;

	//controllers
	private Transform blueController;
	private Transform orangeController;
	//controllers original position
	private Vector3 originalBlueController;
	private Vector3 originalOrangeController;

	//triggers
	private GameObject leftBlueTrigger;
	private GameObject rightBlueTrigger;
	private GameObject leftOrangeTrigger;
	private GameObject rightOrangeTrigger;

	//spawn points
	private Transform blueSpawnPoint;
	private Transform orangeSpawnPoint;
	//spawn points original position
	private Vector3 originalBlueSpawnPoint;
	private Vector3 originalOrangeSpawnPoint;

	//portal sprites
	private Transform leftBlueSprite;
	private Transform righttBlueSprite;
	private Transform leftOrangeSprite;
	private Transform rightOrangeSprite;

	//curtain sprites
	private SpriteRenderer leftBlueCurtain;
	private SpriteRenderer rightBlueCurtain;
	private SpriteRenderer leftOrangeCurtain;
	private SpriteRenderer rightOrangeCurtain;

	//lists of children
	private List<Transform> blueChildren;
	private List<Transform> orangeChildren;


	private void Awake()
	{
		blueChildren = bluePortal.transform.GetAllChildren();
		orangeChildren = orangePortal.transform.GetAllChildren();

		blueController = blueChildren[0];
		orangeController = orangeChildren[0];
		originalBlueController = blueController.position;
		originalOrangeController = orangeController.position;

		leftBlueSprite = blueChildren[1];
		leftOrangeSprite = orangeChildren[1];
		righttBlueSprite = blueChildren[2];
		rightOrangeSprite = orangeChildren[2];

		blueSpawnPoint = blueChildren[3];
		orangeSpawnPoint = orangeChildren[3];
		originalBlueSpawnPoint = blueChildren[3].position;
		originalOrangeSpawnPoint = orangeChildren[3].position;

		leftBlueTrigger = blueChildren[4].gameObject;
		leftOrangeTrigger = orangeChildren[4].gameObject;
		rightBlueTrigger = blueChildren[5].gameObject;
		rightOrangeTrigger = orangeChildren[5].gameObject;

		leftBlueCurtain = leftBlueTrigger.GetComponentInChildren<SpriteRenderer>();
		rightBlueCurtain = rightBlueTrigger.GetComponentInChildren<SpriteRenderer>();
		leftOrangeCurtain = leftOrangeTrigger.GetComponentInChildren<SpriteRenderer>();
		rightOrangeCurtain = rightOrangeTrigger.GetComponentInChildren<SpriteRenderer>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.name == "Player")
		{
			//player entering from left blue or right orange side
			if (ReferenceEquals(gameObject, leftBlueTrigger) || ReferenceEquals(gameObject, rightOrangeTrigger))
			{
				AdjustCurtains(false);
				AdjustControllers(false);
				AdjustPortals(false);
				AdjustSpawnPoints(false);
			}
			//player entering from right blue or left orange side
			else if (ReferenceEquals(gameObject, rightBlueTrigger) || ReferenceEquals(gameObject, leftOrangeTrigger))
			{
				AdjustCurtains(true);
				AdjustControllers(true);
				AdjustPortals(true);
				AdjustSpawnPoints(true);
			}
		}
	}

	private void AdjustPortals(bool reversed)
	{
		var rotationFactor = reversed ? -1f : 1f;

		var tempLeftBlue = leftBlueSprite.transform.position;
		var tempRightBlue = righttBlueSprite.transform.position;
		var tempLeftOrange = leftOrangeSprite.transform.position;
		var tempRightOrange = rightOrangeSprite.transform.position;

		tempLeftBlue.z = rotationFactor;
		tempRightBlue.z = -rotationFactor;
		tempLeftOrange.z = -rotationFactor;
		tempRightOrange.z = rotationFactor;

		leftBlueSprite.transform.position = tempLeftBlue;
		righttBlueSprite.transform.position = tempRightBlue;
		leftOrangeSprite.transform.position = tempLeftOrange;
		rightOrangeSprite.transform.position = tempRightOrange;
	}

	private void AdjustCurtains(bool reversed)
	{
		var tempBool = !reversed;

		rightBlueCurtain.enabled = tempBool;
		leftOrangeCurtain.enabled = tempBool;

		leftBlueCurtain.enabled = !tempBool;
		rightOrangeCurtain.enabled = !tempBool;
	}

	private void AdjustSpawnPoints(bool reversed)
	{
		var tempHeightControl = 0.7f;
		var tempWidthControl = reversed ? -1f : 1f;

		var blueTemp = originalBlueSpawnPoint;
		blueTemp.x += tempWidthControl;
		blueTemp.y -= tempHeightControl;
		blueSpawnPoint.position = blueTemp;

		var orangeTemp = originalOrangeSpawnPoint;
		orangeTemp.x -= tempWidthControl;
		orangeTemp.y -= tempHeightControl;
		orangeSpawnPoint.position = orangeTemp;
	}

	private void AdjustControllers(bool reversed)
	{
		var tempWidthControl = reversed ? -0.5f : 0.5f;

		var blueTemp = originalBlueController;
		blueTemp.x += tempWidthControl;
		blueController.position = blueTemp;

		var orangeTemp = originalOrangeController;
		orangeTemp.x -= tempWidthControl;
		orangeController.position = orangeTemp;
	}
}