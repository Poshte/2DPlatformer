using UnityEngine;

public class Controller2D : RaycastController
{
	//dependencies
	[SerializeField] private LayerMask collisionMask;
	public ControllerInfos info;
	private const float maxSlopeAngle = 60f;
	private Vector2 velocityOld;
	private bool jumpDown;

	public override void Start()
	{
		base.Start();
	}

	public void Move(Vector2 velocity, bool downCommand = false, bool standingOnPlatform = false)
	{
		UpdateRaycastOrigin();
		info.Reset();

		jumpDown = downCommand;

		if (info.bottomCollision)
			velocityOld = velocity;

		if (velocity.y < 0)
			DescendSlope(ref velocity);

		if (velocity.x != 0)
			info.faceDirection = (int)Mathf.Sign(velocity.x);

		HorizontalCollision(ref velocity);

		if (velocity.y != 0)
			VerticalCollision(ref velocity);

		transform.Translate(velocity);

		if (standingOnPlatform)
			info.bottomCollision = true;
	}

	public void HorizontalCollision(ref Vector2 velocity)
	{
		float directionX = info.faceDirection;
		var rayLength = Mathf.Abs(velocity.x) + skinWidth;

		for (int i = 0; i < HorizontalRayCount; i++)
		{
			var rayOrigin = (directionX < 0) ? RaycastOrigin.bottomLeft : RaycastOrigin.bottomRight;
			rayOrigin += Vector2.up * (HorizontalRaySpace * i);
			var hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

			Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);

			if (hit)
			{
				Debug.Log("hit something");

				if (hit.distance == 0f)
				{
					Debug.Log("hit distance is 0");
					continue;
				}

				var slopeAngle = Vector2.Angle(Vector2.up, hit.normal);

				if (i == 0 && slopeAngle <= maxSlopeAngle)
				{
					if (info.descendingSlope)
					{
						info.descendingSlope = false;
						velocity = velocityOld;
					}

					var distanceToSlope = 0f;
					if (slopeAngle != info.OldSlopeAngle)
					{
						distanceToSlope = hit.distance - skinWidth;
						velocity.x -= distanceToSlope * directionX;
					}

					ClimbSlope(ref velocity, slopeAngle, hit.normal);
					velocity.x += distanceToSlope * directionX;
				}
				else
				{
					velocity.x = (hit.distance - skinWidth) * directionX;
					rayLength = hit.distance;

					if (info.climbingSlope)
					{
						velocity.y = Mathf.Tan(info.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
					}

					info.leftCollision = directionX == -1;
					info.rightCollision = directionX == 1;
				}
			}
		}
	}

	public void VerticalCollision(ref Vector2 velocity)
	{
		var directionY = Mathf.Sign(velocity.y);
		var rayLength = Mathf.Abs(velocity.y) + skinWidth;

		for (int i = 0; i < VerticalRayCount; i++)
		{
			var rayOrigin = (directionY < 0) ? RaycastOrigin.bottomLeft : RaycastOrigin.topLeft;
			rayOrigin += Vector2.right * (VerticalRaySpace * i + velocity.x);
			var hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

			Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);

			if (hit)
			{
				if (hit.collider.tag == "Hollow" && directionY == 1)
					continue;

				if (hit.collider.tag != "Solid" && jumpDown)
				{
					continue;
				}

				if (hit.distance != 0f) //this is a custom change
				{
					velocity.y = (hit.distance - skinWidth) * directionY;
					rayLength = hit.distance;

					if (info.climbingSlope)
						velocity.x = velocity.y / Mathf.Tan(info.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);

					info.bottomCollision = directionY == -1;
					info.topCollision = directionY == 1;
				}
			}
		}

		if (info.climbingSlope)
		{
			var directionX = Mathf.Sign(velocity.x);
			rayLength = Mathf.Abs(velocity.x) + skinWidth;
			var rayOrigin = ((directionX < 0) ?
				RaycastOrigin.bottomLeft : RaycastOrigin.bottomRight) + Vector2.up * velocity.y;
			var hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

			if (hit)
			{
				var slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

				if (slopeAngle != info.slopeAngle)
				{
					velocity.x = (hit.distance - skinWidth) * directionX;
					info.slopeAngle = slopeAngle;
					info.slopeNormal = hit.normal;
				}
			}
		}
	}

	public void ClimbSlope(ref Vector2 velocity, float slopeAngle, Vector2 slopeNormal)
	{
		var moveDistance = Mathf.Abs(velocity.x);
		var climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

		if (velocity.y <= climbVelocityY)
		{
			velocity.y = climbVelocityY;
			velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
			info.bottomCollision = true;
			info.climbingSlope = true;
			info.slopeAngle = slopeAngle;
			info.slopeNormal = slopeNormal;
		}
	}

	public void DescendSlope(ref Vector2 velocity)
	{
		var hitLeft = Physics2D.Raycast(RaycastOrigin.bottomLeft, Vector2.down, Mathf.Abs(velocity.y) + skinWidth, collisionMask);
		var hitRight = Physics2D.Raycast(RaycastOrigin.bottomRight, Vector2.down, Mathf.Abs(velocity.y) + skinWidth, collisionMask);
		if (hitLeft ^ hitRight)
		{
			SlideSlope(ref velocity, hitLeft);
			SlideSlope(ref velocity, hitRight);
		}

		if (!info.slidingMaxSlope)
		{
			var directionX = Mathf.Sign(velocity.x);
			var rayOrigin = (directionX < 0) ? RaycastOrigin.bottomRight : RaycastOrigin.bottomLeft;
			var hit = Physics2D.Raycast(rayOrigin, Vector2.down, Mathf.Infinity, collisionMask);

			if (hit)
			{
				var slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

				if (slopeAngle != 0 &&
						slopeAngle <= maxSlopeAngle &&
						directionX == Mathf.Sign(hit.normal.x) &&
						hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
				{
					var moveDistance = Mathf.Abs(velocity.x);
					var descendVelocityY = Mathf.Sign(slopeAngle * Mathf.Deg2Rad) * moveDistance;
					velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * directionX;
					velocity.y -= descendVelocityY;

					info.bottomCollision = true;
					info.descendingSlope = true;
					info.slopeAngle = slopeAngle;
					info.slopeNormal = hit.normal;

				}
			}
		}
	}

	public void SlideSlope(ref Vector2 velocity, RaycastHit2D hit)
	{
		if (hit)
		{
			var slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

			if (slopeAngle > maxSlopeAngle)
			{
				velocity.x = hit.normal.x * (Mathf.Abs(velocity.y) - hit.distance) / Mathf.Tan(slopeAngle * Mathf.Deg2Rad);
				velocity.x /= 2;
				velocity.y /= 2;

				info.slopeAngle = slopeAngle;
				info.slopeNormal = hit.normal;
				info.slidingMaxSlope = true;
			}
		}
	}
}