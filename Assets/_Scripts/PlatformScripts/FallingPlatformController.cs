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
	private ColorService colorService;

	[SerializeField]
	private Color startColor;
	[SerializeField]
	private Color endColor;
	[SerializeField]
	private float colorChangeSpeed;

	public override void Start()
	{
		base.Start();
		colorService = new ColorService();
		platformRenderer = GetComponent<Renderer>();
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
			var hits = Physics2D.RaycastAll(rayOrigin, Vector2.up, skinWidth, playerMask);
			foreach (var hit in hits)
			{

				StartCoroutine(colorService.ChangeColor(platformRenderer, startColor, endColor, 1.5f));

				//StartCoroutine(ChangeColor());
				Invoke(nameof(Fall), fallDelay);
				break;
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

	public IEnumerator ChangeColor()
	{
		var tick = 0f;
		while (platformRenderer.material.color != endColor)
		{
			tick += Time.deltaTime * colorChangeSpeed;
			platformRenderer.material.color = Color.Lerp(startColor, endColor, tick);
			yield return null;
		}
	}
}
