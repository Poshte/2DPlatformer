using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RaycastController : MonoBehaviour
{
	//dependencies
	[HideInInspector]
	public BoxCollider2D boxCollider;
	public RaycastOrigin raycastOrigin;

	//rays
	public const float skinWidth = 0.015f;
	private const float spaceBetweenRays = 0.2f;

    [HideInInspector]
	public int horizontalRayCount;
	[HideInInspector]
    public int verticalRayCount;
    [HideInInspector]
	public float horizontalRaySpace;
	[HideInInspector]
	public float verticalRaySpace;

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

		var height = bounds.size.y;
		var width = bounds.size.x;

		horizontalRayCount = Mathf.RoundToInt(height / spaceBetweenRays);
        verticalRayCount = Mathf.RoundToInt(width / spaceBetweenRays);

        horizontalRaySpace = height / (horizontalRayCount - 1);
        verticalRaySpace = width / (verticalRayCount - 1);

    }
}
public struct RaycastOrigin
{
    public Vector2 topLeft, topRight;
    public Vector2 bottomLeft, bottomRight;
}