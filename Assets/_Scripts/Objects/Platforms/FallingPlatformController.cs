using UnityEngine;

public class FallingPlatformController : RaycastController
{
	[SerializeField] private LayerMask playerMask;
	private bool playerDetected;

	//falling
	[SerializeField] private float fallDelay;
	[SerializeField] private float respawnDelay;

	//colors
	[SerializeField] private Color startColor;
	[SerializeField] private Color endColor;
	[SerializeField] private float colorChangeSpeed;

	//dependencies
	private SpriteRenderer platformRenderer;

	//services
	private IColorService colorService;

	private Player playerScript;

	public override void Start()
	{
		base.Start();
		platformRenderer = GetComponent<SpriteRenderer>();
		colorService = ServiceLocator.Instance.Get<IColorService>();

		playerScript = FindObjectOfType<Player>().GetComponent<Player>();

	}

	void Update()
	{
		UpdateRaycastOrigin();
		DetectPlayer();
	}

	private void DetectPlayer()
	{
		if (playerDetected)
			return;

		for (int i = 0; i < VerticalRayCount; i++)
		{
			var rayOrigin = RaycastOrigin.topLeft;
			rayOrigin += Vector2.right * (VerticalRaySpace * i);

			//another approach is to use raycastall
			//and put the result in a loop
			var hit = Physics2D.Raycast(rayOrigin, Vector2.up, 0.1f, playerMask);

			if (hit)
			{
				bool isFalling = playerScript.GetPlayerVelocity().y <= 0f;
				bool isAbovePlatform = hit.collider.bounds.min.y >= rayOrigin.y;

				if (isFalling && isAbovePlatform)
				{
					playerDetected = true;
					StartCoroutine(colorService.ChangeColor(platformRenderer, startColor, endColor, 1.5f));
					Invoke(nameof(Fall), fallDelay);
				}
			}
		}
	}

	public void Fall()
	{
		gameObject.SetActive(false);
		platformRenderer.color = startColor;
		Invoke(nameof(Respawn), respawnDelay);
	}

	public void Respawn()
	{
		//TODO
		//this approach simply activates and deactivates the game object.
		//it could potentially be better to destroy the game object and reintialize it.
		gameObject.SetActive(true);
		playerDetected = false;
	}
}
