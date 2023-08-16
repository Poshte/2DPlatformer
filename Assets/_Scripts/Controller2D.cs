using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public partial class Controller2D : MonoBehaviour
{
    private BoxCollider2D collider;
    private RaycastOrigin raycastOrigin;
    private Bounds bounds;
    private const float skinWidth = 0.015f;
    private float horizontalRaySpace;
    private float verticalRaySpace;

    [SerializeField]
    private int horizontalRayCount = 4;

	[SerializeField]
	private int verticalRayCount = 4;

	// Start is called before the first frame update
	void Start()
    {
        collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        GetRaycastOrigin();
        GetRaySpacing();

        for (int i = 0; i < verticalRayCount; i++)
        {
            Debug.DrawRay(raycastOrigin.bottomLeft + Vector2.right * verticalRaySpace * i, Vector2.up * -2, Color.red);
            Debug.DrawRay(raycastOrigin.bottomRight + Vector2.up * horizontalRaySpace * i, Vector2.left * -2, Color.red);
            Debug.DrawRay(raycastOrigin.bottomLeft + Vector2.up * horizontalRaySpace * i, Vector2.right * -2, Color.red);
		}
	}

	public void GetRaycastOrigin()
	{
		bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigin.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigin.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigin.topRight = new Vector2(bounds.max.x, bounds.max.y);
        raycastOrigin.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
	}

    public void GetRaySpacing()
    {
		bounds = collider.bounds;
		bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpace = bounds.size.x / (horizontalRayCount - 1);
        verticalRaySpace = bounds.size.y / (verticalRayCount - 1);
	}
}
