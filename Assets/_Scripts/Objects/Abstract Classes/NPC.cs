using UnityEngine;
using UnityEngine.InputSystem;

public abstract class NPC : MonoBehaviour, IIntractable
{
	[SerializeField] private SpriteRenderer interactSprite;
	
	private Transform playerTrnasform;

	private const float interactDistance = 2f;

	void Awake()
	{
		playerTrnasform = GameObject.FindGameObjectWithTag("Player").transform;
	}

	void Update()
	{
		if (PlayerStandingNear())
		{
			interactSprite.gameObject.SetActive(true);

			if (Keyboard.current.eKey.wasPressedThisFrame)
			{
				Interact();
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
		if (UnityEngine.Vector2.Distance(playerTrnasform.position, gameObject.transform.position) < interactDistance)
			return true;
		else
			return false;
	}
}
