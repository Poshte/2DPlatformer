using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RaycastController : MonoBehaviour
{
	//rays
	public const float skinWidth = 0.015f;
	public int horizontalRayCount = 4;
	public int verticalRayCount = 4;

	[HideInInspector]
	public float horizontalRaySpace;
	[HideInInspector]
	public float verticalRaySpace;

	[HideInInspector]
	public BoxCollider2D boxCollider;
	public RaycastOrigin raycastOrigin;


	// Start is called before the first frame update
	public virtual void Start()
	{
		boxCollider = GetComponent<BoxCollider2D>();
		GetRaySpacing();
	}

	public void UpdateRaycastOrigin()
	{
		var bounds = boxCollider.bounds;
		bounds.Expand(skinWidth * -2);

		raycastOrigin.topLeft = new Vector2(bounds.min.x, bounds.max.y);
		raycastOrigin.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
		raycastOrigin.topRight = new Vector2(bounds.max.x, bounds.max.y);
		raycastOrigin.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
	}

	public void GetRaySpacing()
	{
		var bounds = boxCollider.bounds;
		bounds.Expand(skinWidth * -2);

		horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
		verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

		horizontalRaySpace = bounds.size.y / (horizontalRayCount - 1);
		verticalRaySpace = bounds.size.x / (verticalRayCount - 1);
	}
}