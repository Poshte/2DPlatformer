using Assets._Scripts.BaseInfos;
using FMODUnity;
using System;
using UnityEngine;

public class TeleportController : MonoBehaviour
{
	//player prefab
	[SerializeField] private GameObject playerPrefab;

	//clone
	private GameObject clone;

	//dependencies
	private Controller2D controller2D;

	//controllers
	[SerializeField] private GameObject blueController;
	[SerializeField] private GameObject orangeController;
	private Collider2D blueCollider;
	private Collider2D orangeCollider;

	//spawn points
	[SerializeField] private Transform blueSpawnPoint;
	[SerializeField] private Transform orangeSpawnPoint;

	//player's direction when interacting with portal
	private float enterDirection;
	private float exitDirection;

	private void Awake()
	{
		blueCollider = blueController.GetComponent<Collider2D>();
		orangeCollider = orangeController.GetComponent<Collider2D>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag(Constants.Tags.Player))
		{
			controller2D = GameObject.FindGameObjectWithTag(Constants.Tags.Player).GetComponent<Controller2D>();
			enterDirection = controller2D.info.faceDirection;

			//if player enters blue portal
			if (gameObject == blueController)
			{
				DisableCollider(orangeCollider);
				CreateClone(orangeSpawnPoint);
				PlayTeleportSFX();
			}
			//if player enters orange portal
			else if (gameObject == orangeController)
			{
				DisableCollider(blueCollider);
				CreateClone(blueSpawnPoint);
				PlayTeleportSFX();
			}
		}
	}

	private void PlayTeleportSFX()
	{
		AudioManager.Instance.PlayOneShot(FMODEvents.Instance.PortalTeleport, this.transform.position);
		AudioManager.Instance.DisableAllSounds();
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		exitDirection = controller2D.info.faceDirection;

		//if player exits portal without teleporting
		if (enterDirection != exitDirection)
		{
			Destroy(clone);
		}
		//if player teleports with portal
		else
		{
			Destroy(collision.gameObject);
			EnableColliders();
			clone.tag = Constants.Tags.Player;
		}
	}

	private void CreateClone(Transform spawnPoint)
	{
		clone = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
		clone.tag = Constants.Tags.Clone;
	}

	private void DisableCollider(Collider2D collider)
	{
		collider.enabled = false;
	}

	private void EnableColliders()
	{
		blueCollider.enabled = true;
		orangeCollider.enabled = true;
	}
}
