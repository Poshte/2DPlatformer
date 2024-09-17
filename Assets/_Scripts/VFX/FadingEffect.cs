using Assets._Scripts.BaseInfos;
using UnityEngine;

public class FadingEffect : MonoBehaviour
{
	[SerializeField]
	private float fadeDistance;

	/// <summary>
	/// for some reason the shader won't fade in completely 
	/// even when the alpha value is set to 1
	/// so we have to multiply it a bit to cover it up
	/// </summary>
	private const float fadingMultiplier = 1.5f;

	private Material material;
	private Transform playerTransform;

	void Start()
	{
		material = gameObject.GetComponent<Renderer>().sharedMaterial;
		playerTransform = GameObject.FindGameObjectWithTag(Constants.Tags.Player).GetComponent<Transform>();

		material.SetFloat("_Degree", 0);
		material.SetFloat("_FadeAmount", 0);
	}

	void Update()
	{
		//TODO
		//playerTransform gets destroyed after teleportation..OOPS!
		//might be redundant as im not sure yet if i will have FadingEffect and portals in the same level
		if (playerTransform == null)
			playerTransform = GameObject.FindGameObjectWithTag(Constants.Tags.Player).GetComponent<Transform>();

		var distance = Vector2.Distance(playerTransform.position, transform.position);

		if (distance < fadeDistance)
		{
			//find the angle between player and this object
			Vector2 direcion = transform.position - playerTransform.position;
			var angle = Mathf.Atan2(direcion.y, direcion.x) * Mathf.Rad2Deg;

			//normlaize the angle
			if (angle < 0)
				angle += 360;

			var alpha = Mathf.Clamp01(1 - (distance / fadeDistance));
			material.SetFloat("_Degree", angle);
			material.SetFloat("_FadeAmount", alpha * fadingMultiplier);
		}
	}
}
