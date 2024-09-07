using Assets._Scripts.BaseInfos;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class NPC : MonoBehaviour, IIntractable
{
	[SerializeField] private SpriteRenderer interactSprite;
	
	private Transform playerTrnasform;

	private const float interactDistance = 2f;

	void Awake()
	{
		playerTrnasform = GameObject.FindGameObjectWithTag(Constants.Tags.Player).transform;
	}

	void Update()
	{
		if (PlayerStandingNear())
		{
			interactSprite.gameObject.SetActive(true);

			if (Keyboard.current.eKey.wasPressedThisFrame)
			{
				Interact();

				//testing AudioManager
				AudioManager.Instance.PlayOneShot(FMODEvents.Instance.InteractSound, this.transform.position);
			}
		}
		else
		{
			interactSprite.gameObject.SetActive(false);
		}
	}

	public abstract void Interact();

	private bool PlayerStandingNear()
	{
		return Vector2.Distance(playerTrnasform.position, gameObject.transform.position) < interactDistance;
	}
}
