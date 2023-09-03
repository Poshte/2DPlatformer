using UnityEngine;

public partial class Controller2D : RaycastController
{
	//dependencies
	public CollisionDetector collisionDetector;

	private const float maxClimbAngle = 75f;
	private const float maxDescendAngle = 75f;

	private Vector3 velocityOld;

	[SerializeField]
	private LayerMask collisionMask;

	public override void Start()
	{
		base.Start();
	}

	public void Move(Vector3 velocity, bool standingOnPlatform = false)
	{
		UpdateRaycastOrigin();
		collisionDetector.Reset();

		if (collisionDetector.bottomCollision)
		{
			velocityOld = velocity;
		}


		if (velocity.y < 0)
			DescendSlope(ref velocity);

		if (velocity.x != 0)
			HorizontalCollision(ref velocity);

		if (velocity.y != 0)
			VerticalCollision(ref velocity);

		transform.Translate(velocity);

		if (standingOnPlatform)
			collisionDetector.bottomCollision = true;
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
				if (hit.distance == 0f)
					continue;

				var slopeAngle = Vector2.Angle(Vector2.up, hit.normal);

				if (i == 0 && slopeAngle <= maxClimbAngle)
				{
					if (collisionDetector.descendingSlope)
					{
						collisionDetector.descendingSlope = false;
						velocity = velocityOld;
					}

					var distanceToSlope = 0f;
					if (slopeAngle != collisionDetector.OldSlopeAngle)
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
						velocity.y = Mathf.Tan(collisionDetector.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
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
					velocity.x = velocity.y / Mathf.Tan(collisionDetector.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);

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
				var slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

				if (slopeAngle != collisionDetector.slopeAngle)
				{
					velocity.x = (hit.distance - skinWidth) * directionX;
					collisionDetector.slopeAngle = slopeAngle;
				}
			}
		}
	}

	public void ClimbSlope(ref Vector3 velocity, float slopeAngle)
	{
		var moveDistance = Mathf.Abs(velocity.x);
		var climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

		if (velocity.y <= climbVelocityY)
		{
			velocity.y = climbVelocityY;
			velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
			collisionDetector.bottomCollision = true;
			collisionDetector.climbingSlope = true;
			collisionDetector.slopeAngle = slopeAngle;
		}
	}

	public void DescendSlope(ref Vector3 velocity)
	{
		var directionX = Mathf.Sign(velocity.x);
		var rayOrigin = (directionX < 0) ? raycastOrigin.bottomRight : raycastOrigin.bottomLeft;
		var hit = Physics2D.Raycast(rayOrigin, Vector2.down, Mathf.Infinity, collisionMask);

		if (hit)
		{
			var slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

			if (slopeAngle != 0 &&
				slopeAngle <= maxDescendAngle &&
				directionX == Mathf.Sign(hit.normal.x) &&
				hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
			{
				var moveDistance = Mathf.Abs(velocity.x);
				var descendVelocityY = Mathf.Sign(slopeAngle * Mathf.Deg2Rad) * moveDistance;
				velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * directionX;
				velocity.y -= descendVelocityY;

				collisionDetector.bottomCollision = true;
				collisionDetector.descendingSlope = true;
				collisionDetector.slopeAngle = slopeAngle;
			}
		}
	}
}
