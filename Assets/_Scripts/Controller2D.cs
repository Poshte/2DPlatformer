using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public partial class Controller2D : MonoBehaviour
{
    //dependencies
    private BoxCollider2D playerCollider;
    private RaycastOrigin raycastOrigin;
    public CollisionDetector collisionDetector;

    //world objects
	[SerializeField]
	private LayerMask collisionMask;
	private Bounds bounds;

    //rays
	[SerializeField]
	private int horizontalRayCount = 4;
	[SerializeField]
	private int verticalRayCount = 4;
    private const float skinWidth = 0.015f;
    private float horizontalRaySpace;
    private float verticalRaySpace;

    //slope
    private float slopeAngle;
    private const float maxClimbAngel = 60f;

	// Start is called before the first frame update
	void Start()
    {
        playerCollider = GetComponent<BoxCollider2D>();

		GetRaySpacing();
	}

	public void Move(Vector3 velocity)
    {
		UpdateRaycastOrigin();
        collisionDetector.Reset();

        if (velocity.x != 0)
            HorizontalCollision(ref velocity);

        if (velocity.y != 0)
			VerticalCollision(ref velocity);

        transform.Translate(velocity);
    }

	public void HorizontalCollision(ref Vector3 velocity)
	{
		var directionX = Mathf.Sign(velocity.x);
		var rayLength = Mathf.Abs(velocity.x) + skinWidth;

		for (int i = 0; i < horizontalRayCount; i++)
        {
            var rayOrigin = (directionX < 0) ? raycastOrigin.bottomLeft : raycastOrigin.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpace * i);
            var hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit)
            {
				slopeAngle = Vector2.Angle(Vector2.up, hit.normal);

                if (i == 0 && slopeAngle <= maxClimbAngel)
                {
                    var distanceToSlope = 0f;
                    if (slopeAngle != collisionDetector.OldSlopeAngel)
                    {
                        distanceToSlope = hit.distance - skinWidth;
                        velocity.x -= distanceToSlope * directionX;
                    }
                    ClimbSlope(ref velocity, slopeAngle);
                    velocity.x += distanceToSlope * directionX;
                }
                else
                {
					velocity.x = (hit.distance - skinWidth) * directionX;
					rayLength = hit.distance;

                    if (collisionDetector.climbingSlope)
                    {
						velocity.y = Mathf.Tan(collisionDetector.slopeAngel * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
					}

					collisionDetector.leftCollision = directionX == -1;
					collisionDetector.rightCollision = directionX == 1;
				}
			}
        }
	}

	public void VerticalCollision(ref Vector3 velocity)
    {
        var directionY = Mathf.Sign(velocity.y);
		var rayLength = Mathf.Abs(velocity.y) + skinWidth;

		for (int i = 0; i < verticalRayCount; i++)
        {
			var rayOrigin = (directionY < 0) ? raycastOrigin.bottomLeft : raycastOrigin.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpace * i + velocity.x);
			var hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

			if (hit)
			{
				velocity.y = (hit.distance - skinWidth) * directionY;
				rayLength = hit.distance;

                if (collisionDetector.climbingSlope)
                {
					velocity.x = velocity.y / Mathf.Tan(collisionDetector.slopeAngel * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
				}

				collisionDetector.bottomCollision = directionY == -1;
				collisionDetector.topCollision = directionY == 1;
			}
		}

        if (collisionDetector.climbingSlope)
        {
            var directionX = Mathf.Sign(velocity.x);
            rayLength = Mathf.Abs(velocity.x) + skinWidth;
            var rayOrigin = ((directionX < 0) ? 
                raycastOrigin.bottomLeft : raycastOrigin.bottomRight) + Vector2.up * velocity.y;
            var hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            if (hit)
            {
                slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle != collisionDetector.slopeAngel)
                {
					velocity.x = (hit.distance - skinWidth) * directionX;
                    collisionDetector.slopeAngel = slopeAngle;
				}
			}
        }
    }

    public void ClimbSlope(ref Vector3 velocity, float slopeAngle)
    {
        var moveDistance = Mathf.Abs(velocity.x);
        var climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        if (climbVelocityY >= velocity.y)
        {
            velocity.y = climbVelocityY;
			velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
			collisionDetector.bottomCollision = true;
            collisionDetector.climbingSlope = true;
            collisionDetector.slopeAngel = slopeAngle;
		}
	}

	public void UpdateRaycastOrigin()
	{
		bounds = playerCollider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigin.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigin.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigin.topRight = new Vector2(bounds.max.x, bounds.max.y);
        raycastOrigin.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
	}

    public void GetRaySpacing()
    {
		bounds = playerCollider.bounds;
		bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpace = bounds.size.x / (horizontalRayCount - 1);
        verticalRaySpace = bounds.size.y / (verticalRayCount - 1);
	}
}
