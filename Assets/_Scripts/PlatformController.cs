using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : RaycastController
{
	[SerializeField]
	private LayerMask passengerMask;
	[SerializeField]
	private Vector3 move;

	public override void Start()
	{
		base.Start();
	}
	void Update()
	{
		UpdateRaycastOrigin();

		var velocity = move * Time.deltaTime;
		MovePassengers(velocity);

		transform.Translate(velocity);
	}

	public void MovePassengers(Vector3 velocity)
	{
		var movedPassengers = new HashSet<Transform>();

		var directionX = Mathf.Sign(velocity.x);
		var directionY = Mathf.Sign(velocity.y);

		//vertically moving platform
		if (velocity.y != 0f)
		{
			var rayLength = Mathf.Abs(velocity.y) + skinWidth;

			for (int i = 0; i < verticalRayCount; i++)
			{
				var rayOrigin = (directionY < 0) ? raycastOrigin.bottomLeft : raycastOrigin.topLeft;
				rayOrigin += Vector2.right * (verticalRaySpace * i);
				var hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, passengerMask);

				if (hit && !movedPassengers.Contains(hit.transform))
				{
					movedPassengers.Add(hit.transform);

					var pushX = (velocity.y >= 0) ? velocity.x : 0f;
					var pushY = velocity.y - (hit.distance - skinWidth) * directionY;

					hit.transform.Translate(new Vector3(pushX, pushY));
				}
			}
		}

		//horizontally moving platform
		if (velocity.x != 0f)
		{
			var rayLength = Mathf.Abs(velocity.x) + skinWidth;

			for (int i = 0; i < horizontalRayCount; i++)
			{
				var rayOrigin = (directionX < 0) ? raycastOrigin.bottomLeft : raycastOrigin.bottomRight;
				rayOrigin += Vector2.up * (horizontalRaySpace * i);
				var hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, passengerMask);

				if (hit && !movedPassengers.Contains(hit.transform))
				{
					movedPassengers.Add(hit.transform);

					float pushX = velocity.x - (hit.distance - skinWidth) * directionX;
					float pushY = 0f;

					hit.transform.Translate(new Vector3(pushX, pushY));
				}
			}
		}

		//when passenger is on top of a horizontally or downward moving platform
		if (directionY < 0 || velocity.x != 0 && velocity.y == 0)
		{
			var rayLength = skinWidth * 2;

			for (int i = 0; i < verticalRayCount; i++)
			{
				var rayOrigin = raycastOrigin.topLeft + Vector2.right * (verticalRaySpace * i);
				var hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, passengerMask);

				if (hit && !movedPassengers.Contains(hit.transform))
				{
					movedPassengers.Add(hit.transform);

					var pushX = velocity.x;
					var pushY = velocity.y;

					hit.transform.Translate(new Vector3(pushX, pushY));
				}
			}
		}
	}
}
