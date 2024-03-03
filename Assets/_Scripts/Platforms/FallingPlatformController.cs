using System.Collections;
using UnityEngine;

public class FallingPlatformController : RaycastController
{
	[SerializeField]
	private LayerMask playerMask;

	//falling
	[SerializeField]
	private float fallDelay;
	[SerializeField]
	private float respawnDelay;

	//colors
	private Renderer platformRenderer;

	[SerializeField]
	private Color startColor;
	[SerializeField]
	private Color endColor;
	[SerializeField]
	private float colorChangeSpeed;
	private IColorService colorService;
	public override void Start()
	{
		base.Start();
		platformRenderer = GetComponent<Renderer>();
		colorService = ServiceLocator.Instance.Get<IColorService>();
	}

	void Update()
	{
		UpdateRaycastOrigin();
		DetectPlayer();
	}

	private void DetectPlayer()
	{
		for (int i = 0; i < VerticalRayCount; i++)
		{
			var rayOrigin = RaycastOrigin.topLeft;
			rayOrigin += Vector2.right * (VerticalRaySpace * i);
			var hit = Physics2D.Raycast(rayOrigin, Vector2.up, skinWidth, playerMask);

			if (hit)
			{
				StartCoroutine(colorService.ChangeColor(platformRenderer, startColor, endColor, 1.5f));
				Invoke(nameof(Fall), fallDelay);
			}
		}
	}

	public void Fall()
	{
		gameObject.SetActive(false);
		platformRenderer.material.color = startColor;
		Invoke(nameof(Respawn), respawnDelay);
	}

	public void Respawn()
	{
		//this approach simply activates and deactivates the game object.
		//it could potentially be better to destroy the game object and reintialize it.
		gameObject.SetActive(true);
	}
}
