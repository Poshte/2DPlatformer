using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class RaycastController : MonoBehaviour
{
	//dependencies
	private BoxCollider2D _boxCollider;
	private RaycastOrigin _raycastOrigin;

	public BoxCollider2D BoxCollider => _boxCollider;
	public RaycastOrigin RaycastOrigin => _raycastOrigin;

	//rays
	public const float skinWidth = 0.015f;
	private const float spaceBetweenRays = 0.2f;

	public int HorizontalRayCount { get; private set; }
	public int VerticalRayCount { get; private set; }
	public float HorizontalRaySpace { get; private set; }
	public float VerticalRaySpace { get; private set; }


	public virtual void Awake()
	{
		_boxCollider = GetComponent<BoxCollider2D>();
	}

	public virtual void Start()
	{
		GetRaySpacing();
	}

	public void UpdateRaycastOrigin()
	{
		var bounds = GetBounds();

		_raycastOrigin.topLeft = new Vector2(bounds.min.x, bounds.max.y);
		_raycastOrigin.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
		_raycastOrigin.topRight = new Vector2(bounds.max.x, bounds.max.y);
		_raycastOrigin.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
	}

	public void GetRaySpacing()
	{
		var bounds = GetBounds();

		var height = bounds.size.y;
		var width = bounds.size.x;

		HorizontalRayCount = Mathf.RoundToInt(height / spaceBetweenRays);
		VerticalRayCount = Mathf.RoundToInt(width / spaceBetweenRays);

		HorizontalRaySpace = height / (HorizontalRayCount - 1);
		VerticalRaySpace = width / (VerticalRayCount - 1);
	}

	private Bounds GetBounds()
	{
		var bounds = BoxCollider.bounds;
		bounds.Expand(skinWidth * -2);
		return bounds;
	}
}

public struct RaycastOrigin
{
	public Vector2 topLeft, topRight;
	public Vector2 bottomLeft, bottomRight;
}